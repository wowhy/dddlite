namespace CQRSInfrastructure.Store
{
  using DDDLite.CQRS.Store.Npgsql;

  public class InventorySnapshotStore : NpgsqlSnapshotStore
  {
    public InventorySnapshotStore(string connectionString) : base(connectionString)
    {
    }
  }
}