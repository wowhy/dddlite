namespace DDDLite.CQRS.Sagas
{
  using System;
  using System.Linq.Expressions;
  using System.Threading.Tasks;
  using DDDLite.CQRS.Commands;
  using DDDLite.Specifications;

  public class SagaRepository<TSaga> : ISagaRepository<TSaga>
    where TSaga : class, ISaga, new()
  {
    private readonly ISagaStore store;
    private readonly ICommandSender sender;

    public SagaRepository(ISagaStore store, ICommandSender sender)
    {
      this.store = store;
      this.sender = sender;
    }

    public Task<TSaga> FindAsync(Expression<Func<TSaga, bool>> predicate, bool includeCompleted = false)
    {
      return this.store.FindAsync(predicate, includeCompleted);
    }

    public Task<TSaga> FindAsync(Specification<TSaga> spec, bool includeCompleted = false)
    {
      return this.store.FindAsync(spec.Expression, includeCompleted);
    }

    public Task<TSaga> GetByIdAsync(Guid sagaId)
    {
      return this.store.FindAsync<TSaga>(k => k.Id == sagaId, true);
    }

    public async Task SaveAsync(TSaga saga)
    {
      await this.store.SaveAsync(saga);
      var commands = saga.FlushUncommittedCommands();
      foreach (var command in commands)
      {
        await sender.SendAsync(command);
      }
    }
  }
}