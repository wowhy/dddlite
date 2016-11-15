namespace Sample.Core.Repository
{
    using DDDLite;
    using DDDLite.Messaging;
    using DDDLite.Repository;
    using DDDLite.Repository.EntityFramework;

    public class SampleDomainRepository<TAggregateRoot> : EFDomainRepository<TAggregateRoot>
        where TAggregateRoot : class, IAggregateRoot
    {
        public SampleDomainRepository(SampleMasterDbContext context, IEventPublisher eventPublisher) : base(context, eventPublisher)
        {
        }
    }
}