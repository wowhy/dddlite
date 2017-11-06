namespace CQRSCore.CRUD.Handlers
{
  using System;
  using System.Threading.Tasks;

  using DDDLite.Repositories;
  using DDDLite.CQRS.Events;
  using DDDLite.CQRS.Repositories;

  using CQRSCore.CRUD.Domain;
  using CQRSCore.EventSource.Events;

  public class InventoryEventHandlers : IEventHandler<InventoryItemCreated>
    , IEventHandler<InventoryItemRenamed>
    , IEventHandler<ItemsCheckedInToInventory>
    , IEventHandler<ItemsRemovedFromInventory>
    , IEventHandler<InventoryItemDeactivated>
  {
    private readonly IRepository<InventoryItem, Guid> repository;

    public InventoryEventHandlers(IRepository<InventoryItem, Guid> repository)
    {
      this.repository = repository;
    }

    public async Task HandleAsync(InventoryItemCreated message)
    {
      Console.WriteLine("InventoryItemCreated: " + message.Id);
      var item = new InventoryItem
      {
        Id = message.Id,
        RowVersion = message.RowVersion,
        CreatedAt = message.Timestamp,
        CreatedById = message.OperatorId,

        CurrentCount = 0,
        Name = message.Name
      };
      await this.repository.AddAsync(item);
      await this.repository.UnitOfWork.CommitAsync();
    }

    public async Task HandleAsync(InventoryItemRenamed message)
    {
      Console.WriteLine("InventoryItemRenamed: " + message.Id);
      var item = await this.repository.GetByIdAsync(message.Id);

      item.RowVersion = message.RowVersion;
      item.LastUpdatedAt = message.Timestamp;
      item.LastUpdatedById = message.OperatorId;

      item.Name = message.NewName;

      await this.repository.UpdateAsync(item);
      await this.repository.UnitOfWork.CommitAsync();
    }

    public async Task HandleAsync(ItemsCheckedInToInventory message)
    {
      Console.WriteLine("ItemsCheckedInToInventory: " + message.Id);
      var item = await this.repository.GetByIdAsync(message.Id);

      item.RowVersion = message.RowVersion;
      item.LastUpdatedAt = message.Timestamp;
      item.LastUpdatedById = message.OperatorId;

      item.CurrentCount += message.Count;

      await this.repository.UpdateAsync(item);
      await this.repository.UnitOfWork.CommitAsync();
    }

    public async Task HandleAsync(ItemsRemovedFromInventory message)
    {
      Console.WriteLine("ItemsRemovedFromInventory: " + message.Id);
      var item = await this.repository.GetByIdAsync(message.Id);

      item.RowVersion = message.RowVersion;
      item.LastUpdatedAt = message.Timestamp;
      item.LastUpdatedById = message.OperatorId;

      item.CurrentCount -= message.Count;

      await this.repository.UpdateAsync(item);
      await this.repository.UnitOfWork.CommitAsync();
    }

    public async Task HandleAsync(InventoryItemDeactivated message)
    {
      Console.WriteLine("InventoryItemDeactivated: " + message.Id);
      var item = await this.repository.GetByIdAsync(message.Id);

      item.RowVersion = message.RowVersion;
      item.LastUpdatedAt = message.Timestamp;
      item.LastUpdatedById = message.OperatorId;

      item.Activated = false;

      await this.repository.UpdateAsync(item);
      await this.repository.UnitOfWork.CommitAsync();
    }
  }
}