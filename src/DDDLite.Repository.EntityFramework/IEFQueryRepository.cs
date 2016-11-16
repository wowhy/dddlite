namespace DDDLite.Repository.EntityFramework
{
    public interface IEFQueryRepository<TAggregateRoot> : IQueryRepository<TAggregateRoot>
        where TAggregateRoot : class, IAggregateRoot
    {
    }
}
