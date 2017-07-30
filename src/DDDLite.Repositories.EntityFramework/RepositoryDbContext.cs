namespace DDDLite.Repositories.EntityFramework
{
    using System;
    using System.Linq;
    using Microsoft.EntityFrameworkCore;
    using DDDLite.Domain;

    public class RepositoryDbContext : DbContext
    {
        public RepositoryDbContext(DbContextOptions options) : base(options)
        {
        }

        public override int SaveChanges()
        {
            OnBeforeSaving();
            return base.SaveChanges();
        }

        public override System.Threading.Tasks.Task<int> SaveChangesAsync(System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken))
        {
            OnBeforeSaving();
            return base.SaveChangesAsync(cancellationToken);
        }

        public override int SaveChanges(bool acceptAllChangesOnSuccess)
        {
            OnBeforeSaving();
            return base.SaveChanges(acceptAllChangesOnSuccess);
        }

        protected virtual void OnBeforeSaving()
        {
            foreach (var entry in ChangeTracker.Entries())
            {
                if (entry.Entity is ILogicalDelete)
                {
                    switch (entry.State)
                    {
                        case EntityState.Added:
                            entry.CurrentValues[nameof(ILogicalDelete.Deleted)] = false;
                            break;
                        case EntityState.Deleted:
                            entry.CurrentValues[nameof(ILogicalDelete.Deleted)] = true;
                            entry.State = EntityState.Modified;
                            break;
                    }
                }
            }
        }
    }
}
