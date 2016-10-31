namespace DDDLite.Repository.EntityFramework
{
    using System;
    using System.Linq;

    using DDDLite.Domain;
    using DDDLite.Specifications;

    public class EFQueryRepository<TAggregateRoot> : QueryRepository<TAggregateRoot>
        where TAggregateRoot : class, IAggregateRoot
    {
        public EFQueryRepository(IQueryRepositoryContext context)
            : base(context)
        {
        }
    }
}