namespace DDDLite.CQRS.Commands
{
  using System;
  using System.Reflection;
  using System.Threading.Tasks;
  using System.Linq;

  using DDDLite.CQRS;
  using DDDLite.CQRS.Repositories;

  public abstract class CommandHandlers<TAggregateRoot>
     where TAggregateRoot : class, IEventSource, new()
  {
    private readonly IDomainRepository<TAggregateRoot> repository;

    protected CommandHandlers(IDomainRepository<TAggregateRoot> repository)
    {
      this.repository = repository;
    }

    public IDomainRepository<TAggregateRoot> Repository => this.repository;

    protected async Task InvokeAsync(ICommand command, Action<TAggregateRoot> action)
    {
      var aggregateRoot = await this.Repository.GetByIdAsync(command.Id, command.OriginalVersion);
      action(aggregateRoot);
      await this.Repository.SaveAsync(aggregateRoot, command.OriginalVersion);
    }

    protected async Task DynamicInvokeAsync(ICommand command)
    {
      var aggregateRoot = await this.Repository.GetByIdAsync(command.Id, command.OriginalVersion);
      var commandType = command.GetType();
      var method = typeof(TAggregateRoot).GetMethods(BindingFlags.Instance | BindingFlags.Public).Where(k => k.GetParameters().FirstOrDefault()?.ParameterType == commandType).First();
      method.Invoke(aggregateRoot, new object[] { command });
      await this.Repository.SaveAsync(aggregateRoot, command.OriginalVersion);
    }
  }
}