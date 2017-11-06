namespace CQRSInfrastructure.Store
{
  using DDDLite.CQRS.Store.Npgsql;
  public class InventoryEventStore : NpgsqlEventStore
  {
    public InventoryEventStore(string connectionString) : base(connectionString)
    {
    }
  }
}