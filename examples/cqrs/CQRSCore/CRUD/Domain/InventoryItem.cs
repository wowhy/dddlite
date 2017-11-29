namespace CQRSCore.CRUD.Domain
{
  using System;
  using DDDLite.CQRS;
  using DDDLite.Domain;

  public class InventoryItem : Dto
  {
    public InventoryItem()
    {
      this.Activated = true;
    }
    public string Name { get; set; }
    public int CurrentCount { get; set; }
    public bool Activated { get; set; }
  }
}