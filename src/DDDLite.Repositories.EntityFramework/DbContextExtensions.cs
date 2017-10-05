namespace DDDLite.Repositories.EntityFramework
{
    using System;
    using System.Linq;
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
                            var originValues = entry.GetDatabaseValues();
                            entry.CurrentValues[nameof(ITrackable.CreatedAt)] = originValues.GetValue<DateTime?>(nameof(ITrackable.CreatedAt));
                            entry.CurrentValues[nameof(ITrackable.CreatedById)] = originValues.GetValue<Guid?>(nameof(ITrackable.CreatedById));
                            break;
                    }
                }
            }
        }

        public static void EnsureConcurrencyCheck(this DbContext context)
        {
            foreach (var entry in context.ChangeTracker.Entries())
            {
                if (entry.Entity is IConcurrencyToken)
                {
                    switch (entry.State)
                    {
                        case EntityState.Deleted:
                            entry.OriginalValues[nameof(IConcurrencyToken.RowVersion)] = entry.CurrentValues[nameof(IConcurrencyToken.RowVersion)];
                            break;
                        case EntityState.Modified:
                            entry.OriginalValues[nameof(IConcurrencyToken.RowVersion)] = entry.CurrentValues[nameof(IConcurrencyToken.RowVersion)];
                            entry.CurrentValues[nameof(IConcurrencyToken.RowVersion)] = (long)entry.CurrentValues[nameof(IConcurrencyToken.RowVersion)] + 1;
                            break;
                    }
                }
            }
        }

        public static EntityTypeBuilder<TEntity> HasQueryFilterLogicalDelete<TEntity>(this EntityTypeBuilder<TEntity> entityBuilder)
            where TEntity : class, ILogicalDelete
        {
            return entityBuilder.HasQueryFilter(k => k.Deleted == false);
        }
    }
}
