namespace CQRSInfrastructure.EntityFramework
{
  using Microsoft.EntityFrameworkCore;

  using CQRSCore.CRUD.Domain;

  public class InventoryDbContext : DbContext
  {
    public InventoryDbContext(DbContextOptions<InventoryDbContext> options) : base(options) { }

    public DbSet<InventoryItem> InventoryItems { get; set; }
  }
}