namespace CQRSInfrastructure.Store
{
  using DDDLite.CQRS.Npgsql;
  public class InventoryEventStore : NpgsqlEventStore
  {
    public InventoryEventStore(string connectionString) : base(connectionString)
    {
    }
  }
}