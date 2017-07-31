namespace DDDLite.Repositories.EntityFramework
{
    using System;
    using System.Threading.Tasks;
    using Microsoft.EntityFrameworkCore;
    using DDDLite.Domain;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    public static class DbContextExtensions
    {
        public static Task<int> SaveChangesWithLogicalDeleteAsync(this DbContext context)
        {
            OnBeforeSaving(context);
            return context.SaveChangesAsync();
        }

        public static EntityTypeBuilder<TEntity> HasQueryFilterLogicalDelete<TEntity>(EntityTypeBuilder<TEntity> entityBuilder)
            where TEntity : class, ILogicalDelete
        {
            return entityBuilder.HasQueryFilter(k => k.Deleted == false);
        }

        private static void OnBeforeSaving(DbContext context)
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
    }
}
