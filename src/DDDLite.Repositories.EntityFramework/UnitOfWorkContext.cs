namespace DDDLite.Repositories.EntityFramework
{
    using System.Threading;
    using System.Threading.Tasks;
    using DDDLite.Repositories;

    using Microsoft.EntityFrameworkCore;

    public class UnitOfWorkContext : DbContext, IUnitOfWork
    {
        public UnitOfWorkContext(DbContextOptions options) : base(options) { }
        protected UnitOfWorkContext() : base() { }

        public async Task CommitAsync(CancellationToken cancellationToken)
        {
            this.EnsureLogicalDeleteChanging();
            this.EnsureConcurrencyCheck();
            this.EnsureTrackableChanging();

            await this.SaveChangesAsync();
        }
    }
}