namespace DDDLite.Querying.EF
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using Specifications;
    using Microsoft.EntityFrameworkCore;
    using System.Reflection;

    public class EFQueryService<TReadModel> : QueryService<TReadModel>, IEFQueryService<TReadModel>
        where TReadModel : class, new()
    {
        private DbContext context;

        public EFQueryService(DbContext context)
        {
            this.context = context;
        }

        public DbContext Context => this.context;

        public virtual IQueryable<TReadModel> NoTrackingSet(string op, string[] eagerLoadings)
        {
            var query = this.context.Set<TReadModel>().AsNoTracking();

            // todo: 通过输入的贪婪加载暂时有严重BUG， 连续两次使用错误的参数调用会造成DbContext查询超时
            //if (eagerLoadings != null)
            //{
            //    foreach (var p in eagerLoadings)
            //    {
            //        query = query.Include(p);
            //    }
            //}

            var properties = typeof(TReadModel).GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.SetProperty)
                .Where(k => k.GetCustomAttribute<EagerLoadingAttribute>() != null);

            foreach (var prop in properties)
            {
                query = query.Include(prop.Name);
            }

            return query;
        }

        public override Task<int> CountAsync(Specification<TReadModel> specification)
        {
            return this.Query(specification).CountAsync();
        }

        public override Task<TReadModel> GetByIdAsync(Guid key, string[] eagerLoadings)
        {
            var query = this.NoTrackingSet("GetById", eagerLoadings);
            return query.FirstOrDefaultAsync(k => EF.Property<Guid>(k, "Id") == key);
        }

        public override IQueryable<TReadModel> Query(Specification<TReadModel> specification, SortSpecification<TReadModel> sortSpecification, string[] eagerLoadings)
        {
            var query = this.NoTrackingSet("Query", eagerLoadings).Where(specification);
            if (sortSpecification != SortSpecification<TReadModel>.None)
            {
                var sorts = sortSpecification.Specifications.ToList();
                var first = sorts[0];

                IOrderedQueryable<TReadModel> ordered;
                if (first.Item2 == SortDirection.Desc)
                {
                    ordered = query.OrderByDescending(first.Item1);
                }
                else
                {
                    ordered = query.OrderBy(first.Item1);
                }

                for (var i = 1; i < sorts.Count; i++)
                {
                    var sort = sorts[i];
                    if (sort.Item2 == SortDirection.Desc)
                    {
                        ordered = query.OrderByDescending(sort.Item1);
                    }
                    else
                    {
                        ordered = query.OrderBy(sort.Item1);
                    }
                }

                return ordered;
            }

            return query;
        }
    }
}
