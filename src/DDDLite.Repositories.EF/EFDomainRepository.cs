namespace DDDLite.Repositories.EF
{
    using Microsoft.EntityFrameworkCore;
    using System;
    using System.Threading.Tasks;

    public class EFDomainRepository<TAggregateRoot> : IEFDomainRepository<TAggregateRoot>
        where TAggregateRoot : class, IAggregateRoot
    {
        private DbContext context;

        public EFDomainRepository(DbContext context)
        {
            this.context = context;
        }

        public DbContext Context => this.context;

        public Task DeleteAsync(TAggregateRoot aggregateRoot)
        {
            var entry = this.Context.Entry(aggregateRoot);
            try
            {
                if (entry.State != EntityState.Deleted)
                {
                    context.Set<TAggregateRoot>().Remove(aggregateRoot);
                }

                return context.SaveChangesAsync();
            }
            catch
            {
                entry.State = EntityState.Detached;
                throw;
            }
        }

        public async Task<TAggregateRoot> GetByIdAsync(Guid key)
        {
            var entity = await this.Context.Set<TAggregateRoot>().FindAsync(key);
            if (entity != null)
            {
                var entry = this.Context.Entry(entity);
                entry.Reload();
            }

            return entity;
        }

        public async Task SaveAsync(TAggregateRoot aggregateRoot)
        {
            var entry = this.Context.Entry(aggregateRoot);
            try
            {
                if (entry.State == EntityState.Detached)
                {
                    context.Set<TAggregateRoot>().Add(aggregateRoot);
                }
                else
                {
                    entry.State = EntityState.Modified;
                }

                await this.Context.SaveChangesAsync();
            }
            catch
            {
                entry.State = EntityState.Detached;
                throw;
            }
        }
    }
}
