namespace DDDLite.CQRS.Sagas
{
  using System;
  using System.Linq.Expressions;
  using System.Threading.Tasks;

  public abstract class SagaHandlers<TSaga> where TSaga : class, ISaga, new()
  {
    private readonly ISagaRepository<TSaga> repository;

    public ISagaRepository<TSaga> Repository => this.repository;

    protected SagaHandlers(ISagaRepository<TSaga> repository)
    {
      this.repository = repository;
    }

    protected async Task HandleAsync(Expression<Func<TSaga, bool>> predicate, Func<TSaga, Task> handler)
    {
      var saga = await this.Repository.FindAsync(predicate);
      if (saga != null)
      {
        await handler(saga);
        await this.Repository.SaveAsync(saga);
      }
    }
  }
}