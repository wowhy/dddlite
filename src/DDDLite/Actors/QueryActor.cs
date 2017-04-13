namespace DDDLite.Actors
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using System.Linq;

    using Querying;

    using Akka.Actor;
    using Validation;

    public abstract class QueryActor : BaseActor
    {
        protected QueryActor()
        {
        }
    }

    public class QueryActor<TReadModel> : QueryActor
        where TReadModel : class, new()
    {
        private IQueryService<TReadModel> service;

        public QueryActor(IQueryService<TReadModel> service)
        {
            this.service = service;
        }

        protected IQueryService<TReadModel> Service => this.service;

        protected override void InitReceiveMessages()
        {
            this.ReceiveAsync<Guid>(this.GetById);
            this.Receive<PagedInputForm>(form => { this.Paged(form); });
            this.Receive<FindAllInputForm>(form => { this.Search(form); });
            this.Receive<FindSingleInputForm>(form => { this.FindSingle(form); });
            this.ReceiveAny(msg => this.TellFailure(new NotImplementedException()));
        }

        public async Task GetById(Guid id)
        {
            try
            {
                var model = await this.service.GetByIdAsync(id);
                if (model is ILogicalDelete && ((ILogicalDelete)model).Deleted)
                {
                    model = null;
                }
                this.TellSuccess<TReadModel>(model);
            }
            catch (Exception ex)
            {
                this.TellFailure<TReadModel>(ex);
            }
        }

        public void Paged(PagedInputForm form)
        {
            try
            {
                if (form.PageIndex < 0)
                {
                    throw new CoreValidateException("“页码”参数不正确！");
                }

                if (form.PageSize <= 0)
                {
                    throw new CoreValidateException("“每页数量”参数不正确！");
                }

                var filters = form.Filters ?? new List<Filter>();
                var sorters = form.Sorters ?? new List<Sorter>();
                var eagerLoadings = form.EagerLoadings ?? new List<string>();

                if (new TReadModel() is ILogicalDelete)
                {
                    filters.Insert(0, new Filter("Deleted", "false"));
                }

                var query = this.service.Query(
                    form.Filters.ToSpecification<TReadModel>(),
                    form.Sorters.ToSpecification<TReadModel>(),
                    eagerLoadings.ToArray());

                var count = query.Count();

                List<TReadModel> data;

                if (form.PageIndex > 0)
                {
                    data = query.Skip((form.PageIndex - 1) * form.PageSize).Take(form.PageSize).ToList();
                }
                else
                {
                    data = query.Take(form.PageSize).ToList();
                }

                this.TellSuccess<PagedResult<TReadModel>>(new PagedResult<TReadModel>
                {
                    Total = count,
                    Data = data
                });
            }
            catch (Exception ex)
            {
                this.TellFailure<PagedResult<TReadModel>>(ex);
            }
        }

        public void Search(FindAllInputForm form)
        {
            try
            {
                var filters = form.Filters ?? new List<Filter>();
                var sorters = form.Sorters ?? new List<Sorter>();
                var eagerLoadings = form.EagerLoadings ?? new List<string>();
                var query = this.service.Query(
                    form.Filters.ToSpecification<TReadModel>(),
                    form.Sorters.ToSpecification<TReadModel>(),
                    eagerLoadings.ToArray());

                var data = query.ToList();

                this.TellSuccess<List<TReadModel>>(data);
            }
            catch (Exception ex)
            {
                this.TellFailure<List<TReadModel>>(ex);
            }
        }

        public void FindSingle(FindSingleInputForm form)
        {
            try
            {
                var filters = form.Filters ?? new List<Filter>();
                var sorters = form.Sorters ?? new List<Sorter>();
                var eagerLoadings = form.EagerLoadings ?? new List<string>();
                var query = this.service.Query(
                    form.Filters.ToSpecification<TReadModel>(),
                    form.Sorters.ToSpecification<TReadModel>(),
                    eagerLoadings.ToArray());

                var data = query.FirstOrDefault();

                this.TellSuccess<TReadModel>(data);
            }
            catch (Exception ex)
            {
                this.TellFailure<TReadModel>(ex);
            }
        }
    }
}