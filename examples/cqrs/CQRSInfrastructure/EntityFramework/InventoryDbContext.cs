namespace CQRSInfrastructure.EntityFramework
{
  using Microsoft.EntityFrameworkCore;

  using CQRSCore.CRUD.Domain;
  using DDDLite.Repositories.EntityFramework;

  public class InventoryDbContext : UnitOfWorkDbContext
  {
    public InventoryDbContext(DbContextOptions<InventoryDbContext> options) : base(options) { }

    public DbSet<InventoryItem> InventoryItems { get; set; }
  }
}