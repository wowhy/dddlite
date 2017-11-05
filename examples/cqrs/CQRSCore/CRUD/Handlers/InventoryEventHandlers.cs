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
      var item = new InventoryItem
      {
        Id = message.AggregateRootId,
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
      var item = await this.repository.GetByIdAsync(message.AggregateRootId);

      item.RowVersion = message.RowVersion;
      item.LastUpdatedAt = message.Timestamp;
      item.LastUpdatedById = message.OperatorId;

      item.Name = message.NewName;

      await this.repository.UpdateAsync(item);
      await this.repository.UnitOfWork.CommitAsync();
    }

    public async Task HandleAsync(ItemsCheckedInToInventory message)
    {
      var item = await this.repository.GetByIdAsync(message.AggregateRootId);

      item.RowVersion = message.RowVersion;
      item.LastUpdatedAt = message.Timestamp;
      item.LastUpdatedById = message.OperatorId;

      item.CurrentCount += message.Count;

      await this.repository.UpdateAsync(item);
      await this.repository.UnitOfWork.CommitAsync();
    }

    public async Task HandleAsync(ItemsRemovedFromInventory message)
    {
      var item = await this.repository.GetByIdAsync(message.AggregateRootId);

      item.RowVersion = message.RowVersion;
      item.LastUpdatedAt = message.Timestamp;
      item.LastUpdatedById = message.OperatorId;

      item.CurrentCount -= message.Count;

      await this.repository.UpdateAsync(item);
      await this.repository.UnitOfWork.CommitAsync();
    }

    public async Task HandleAsync(InventoryItemDeactivated message)
    {
      var item = await this.repository.GetByIdAsync(message.AggregateRootId);

      item.RowVersion = message.RowVersion;
      item.LastUpdatedAt = message.Timestamp;
      item.LastUpdatedById = message.OperatorId;

      item.Activated = false;

      await this.repository.UpdateAsync(item);
      await this.repository.UnitOfWork.CommitAsync();
    }
  }
}