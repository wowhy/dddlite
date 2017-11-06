namespace CQRSCore.EventSource.Handlers
{
  using System;
  using System.Threading.Tasks;

  using DDDLite.CQRS.Commands;
  using DDDLite.CQRS.Repositories;

  using CQRSCore.EventSource.Domain;
  using CQRSCore.EventSource.Commands;

  public class InventoryCommandHandlers : ICommandHandler<CreateInventoryItem>
    , ICommandHandler<RenameInventoryItem>
    , ICommandHandler<DeactivateInventoryItem>
    , ICommandHandler<RemoveItemsFromInventory>
    , ICommandHandler<CheckInItemsToInventory>
  {
    private readonly IDomainRepository<InventoryItem> repository;

    public InventoryCommandHandlers(IDomainRepository<InventoryItem> repository)
    {
      this.repository = repository;
    }

    public async Task HandleAsync(CreateInventoryItem message)
    {
      var item = new InventoryItem(message.Id, message.Name);
      await this.repository.SaveAsync(item);
    }

    public async Task HandleAsync(RenameInventoryItem message)
    {
      var item = await this.repository.GetByIdAsync(message.Id);
      item.ChangeName(message.NewName);
      await this.repository.SaveAsync(item);
    }

    public async Task HandleAsync(DeactivateInventoryItem message)
    {
      var item = await this.repository.GetByIdAsync(message.Id);
      item.Deactivate();
      await this.repository.SaveAsync(item);
    }

    public async Task HandleAsync(CheckInItemsToInventory message)
    {
      var item = await this.repository.GetByIdAsync(message.Id);
      item.CheckIn(message.Count);
      await this.repository.SaveAsync(item);
    }

    public async Task HandleAsync(RemoveItemsFromInventory message)
    {
      var item = await this.repository.GetByIdAsync(message.Id);
      item.Remove(message.Count);
      await this.repository.SaveAsync(item);
    }
  }
}