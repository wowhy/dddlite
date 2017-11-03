namespace DDDLite.CQRS.Repositories
{
  using System;
  using System.Threading.Tasks;

  public interface IDomainRepository<TEventSource>
    where TEventSource : class, IEventSource, new ()
  {
    Task<TEventSource> GetByIdAsync(Guid id);

    Task SaveAsync(TEventSource aggregateRoot);
  }
}