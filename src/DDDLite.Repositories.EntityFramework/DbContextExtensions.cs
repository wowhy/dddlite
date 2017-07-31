namespace DDDLite.Repositories.EntityFramework
{
    using System;
    using System.Threading.Tasks;
    using Microsoft.EntityFrameworkCore;
    using DDDLite.Domain;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    public static class DbContextExtensions
    {
        public static void EnsureLogicalDeleteChanging(this DbContext context)
        {
            foreach (var entry in context.ChangeTracker.Entries())
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

        public static void EnsureTrackableChanging(this DbContext context)
        {
            foreach (var entry in context.ChangeTracker.Entries())
            {
                if (entry.Entity is ITrackable)
                {
                    switch (entry.State)
                    {
                        case EntityState.Modified:
                            entry.CurrentValues[nameof(ITrackable.CreatedAt)] = entry.OriginalValues[nameof(ITrackable.CreatedAt)];
                            entry.CurrentValues[nameof(ITrackable.CreatedById)] = entry.OriginalValues[nameof(ITrackable.CreatedById)];
                            break;
                    }
                }
            }
        }

        public static EntityTypeBuilder<TEntity> HasQueryFilterLogicalDelete<TEntity>(EntityTypeBuilder<TEntity> entityBuilder)
            where TEntity : class, ILogicalDelete
        {
            return entityBuilder.HasQueryFilter(k => k.Deleted == false);
        }
    }
}
