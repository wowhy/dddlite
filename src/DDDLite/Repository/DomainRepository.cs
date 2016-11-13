namespace DDDLite.Repository
{
    using System;
    using System.Linq;

    using Messaging;
    using Specifications;

    public abstract class DomainRepository<TAggregateRoot> : IDomainRepository<TAggregateRoot>
        where TAggregateRoot : class, IAggregateRoot
    {
        private readonly IEventPublisher eventPublisher;

        protected DomainRepository(IEventPublisher eventPublisher)
        {
            this.eventPublisher = eventPublisher;
        }

        public virtual void Save(TAggregateRoot entity)
        {
            this.DoSave(entity);
            if (this.eventPublisher != null)
            {
                foreach (var e in entity.UncommittedEvents)
                {
                    this.eventPublisher.Publish(e);
                }
            }
        }

        protected abstract void DoSave(TAggregateRoot entity);

        public abstract TAggregateRoot GetById(Guid id);

        public virtual IQueryable<TAggregateRoot> Find()
        {
            return this.Find(Specification<TAggregateRoot>.Any(), null);
        }

        public virtual IQueryable<TAggregateRoot> Find(Specification<TAggregateRoot> specification)
        {
            return this.Find(specification, null);
        }

        public abstract IQueryable<TAggregateRoot> Find(Specification<TAggregateRoot> specification, SortSpecification<TAggregateRoot> sortSpecification);

        public abstract bool Exist(Specification<TAggregateRoot> specification);
    }
}
