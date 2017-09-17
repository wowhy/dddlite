namespace DDDLite.Repositories.EntityFramework
{
    using System.Threading;
    using System.Threading.Tasks;
    using DDDLite.Repositories;

    using Microsoft.EntityFrameworkCore;

    public abstract class UnitOfWorkDbContext : DbContext, IUnitOfWork
    {
        public UnitOfWorkDbContext(DbContextOptions options) : base(options) { }

        public override int SaveChanges(bool acceptAllChangesOnSuccess) 
        {
            this.EnsureLogicalDeleteChanging();
            this.EnsureConcurrencyCheck();
            this.EnsureTrackableChanging();

            return base.SaveChanges(acceptAllChangesOnSuccess);
        }

        public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default(CancellationToken))
        {
            this.EnsureLogicalDeleteChanging();
            this.EnsureConcurrencyCheck();
            this.EnsureTrackableChanging();

            return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
        }

        public async Task CommitAsync(CancellationToken cancellationToken)
        {
            await this.SaveChangesAsync();
        }
    }
}