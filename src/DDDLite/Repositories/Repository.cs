namespace DDDLite.Repositories
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Core;
    using Specifications;

    public abstract class Repository<TAggregateRoot> : IRepository<TAggregateRoot>
        where TAggregateRoot : class, IAggregateRoot
    {
        private readonly IRepositoryContext context;

        protected Repository(IRepositoryContext context)
        {
            this.context = context;
        }

        public IRepositoryContext Context => this.context;

        public abstract TAggregateRoot Get(Guid key);

        public abstract Task<TAggregateRoot> GetAsync(Guid key);

        public abstract IQueryable<TAggregateRoot> FindAll();

        public abstract IQueryable<TAggregateRoot> FindAll(Specification<TAggregateRoot> specification);

        public abstract IQueryable<TAggregateRoot> FindAll(Specification<TAggregateRoot> specification, SortSpecification<TAggregateRoot> sortSpecification);

        public abstract void Insert(TAggregateRoot entity);

        public abstract void Update(TAggregateRoot entity);

        public abstract void Delete(TAggregateRoot entity);

        public abstract bool Exists(Specification<TAggregateRoot> specification);
    }
}
