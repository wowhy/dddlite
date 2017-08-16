namespace Example.Repositories.EntityFramework
{
    public class EFRepositoryBase<TAggregateRoot> : DDDLite.Repositories.EntityFramework.EFRepository<TAggregateRoot>
        where TAggregateRoot : class, DDDLite.Domain.IAggregateRoot
    {
        public EFRepositoryBase(ExampleDbContext context) : base(context)
        {
        }        
    }
}