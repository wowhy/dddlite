namespace CQRSCore.CRUD.Domain
{
  using System;

  using DDDLite.Domain;

  public class InventoryItem : AggregateRoot<Guid>
  {
    public InventoryItem()
    {
    }
    public string Name { get; set; }
    public int CurrentCount { get; set; }
    public bool Activated { get; set; }
  }
}