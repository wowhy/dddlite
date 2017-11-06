namespace CQRSCore.EventSource.Commands
{
  using System;
  using DDDLite.CQRS.Commands;

  public class CreateInventoryItem : Command
  {
    public readonly string Name;
    public CreateInventoryItem(string name)
    {
      this.Name = name;
    }
    public CreateInventoryItem(Guid id, string name)
    {
      this.Id = id;
      this.Name = name;
    }
  }
}