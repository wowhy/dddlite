namespace CQRSInfrastructure.Store
{
  using DDDLite.CQRS.Npgsql;

  public class InventorySnapshotStore : NpgsqlSnapshotStore
  {
    public InventorySnapshotStore(string connectionString) : base(connectionString)
    {
    }
  }
}