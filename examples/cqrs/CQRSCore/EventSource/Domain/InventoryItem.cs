namespace CQRSCore.EventSource.Domain
{
  using System;

  using DDDLite.CQRS;

  using CQRSCore.EventSource.Events;
  using CQRSCore.EventSource.Snapshots;

  public class InventoryItem : SnapshotEventSource<InventoryItemSnapshot>
  {
    public InventoryItem() { }

    public InventoryItem(Guid id, string name)
    {
      Id = id;
      ApplyChange(new InventoryItemCreated(id, name));
    }

    public bool Activated { get; set; }

    private void Apply(InventoryItemCreated e)
    {
      Activated = true;
    }

    private void Apply(InventoryItemDeactivated e)
    {
      Activated = false;
    }

    public void ChangeName(string newName)
    {
      if (string.IsNullOrEmpty(newName)) throw new ArgumentException("newName");
      ApplyChange(new InventoryItemRenamed(Id, newName));
    }

    public void Remove(int count)
    {
      if (count <= 0) throw new InvalidOperationException("cant remove negative count from inventory");
      ApplyChange(new ItemsRemovedFromInventory(Id, count));
    }


    public void CheckIn(int count)
    {
      if (count <= 0) throw new InvalidOperationException("must have a count greater than 0 to add to inventory");
      ApplyChange(new ItemsCheckedInToInventory(Id, count));
    }

    public void Deactivate()
    {
      if (!Activated) throw new InvalidOperationException("already deactivated");
      ApplyChange(new InventoryItemDeactivated(Id));
    }

    public override InventoryItemSnapshot GetSnapshot()
    {
      var snapshot = new InventoryItemSnapshot
      {
        Id = Id,
        Version = Version,

        Activated = Activated
      };

      return snapshot;
    }

    public override void RestoreFromSnapshot(InventoryItemSnapshot snapshot)
    {
      this.Id = snapshot.Id;
      this.Version = snapshot.Version;
      this.Activated = snapshot.Activated;
    }
  }
}