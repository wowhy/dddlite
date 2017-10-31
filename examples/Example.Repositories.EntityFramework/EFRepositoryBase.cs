namespace Example.Repositories.EntityFramework
{
    using System;

    public class EFRepositoryBase<TAggregateRoot> : DDDLite.Repositories.EntityFramework.EFRepository<TAggregateRoot, Guid>
        where TAggregateRoot : class, DDDLite.Domain.IAggregateRoot<Guid>
    {
        public EFRepositoryBase(ExampleDbContext context) : base(context)
        {
        }
    }
}