namespace CQRSCore.CRUD.Domain
{
  using System;

  using DDDLite.Domain;

  public class InventoryItem : AggregateRoot<Guid>
  {
    public InventoryItem()
    {
    }
    public string Name;
    public int CurrentCount;
    public bool Activated;
  }
}