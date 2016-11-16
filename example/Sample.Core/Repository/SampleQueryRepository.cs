namespace Sample.Core.Repository
{
    using DDDLite;
    using DDDLite.Messaging;
    using DDDLite.Repository;
    using DDDLite.Repository.EntityFramework;

    public class SampleQueryRepository<TAggregateRoot> : EFQueryRepository<TAggregateRoot>, IEFQueryRepository<TAggregateRoot>
        where TAggregateRoot : class, IAggregateRoot
    {
        public SampleQueryRepository(SampleReadonlyDbContext context) : base(context)
        {
        }
    }
}