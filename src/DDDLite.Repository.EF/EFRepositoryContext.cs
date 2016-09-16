namespace DDDLite.Repository.EF
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using DDDLite.Domain.Repositories;
    using Microsoft.EntityFrameworkCore;

    public class EFRepositoryContext : RepositoryContext
    {
        private readonly DbContext context;
        private bool disposed;

        public EFRepositoryContext(DbContext context)
        {
            this.context = context;
        }

        public DbContext Context => this.context;

        public override void Commit()
        {
            this.context.SaveChanges();
        }

        public override async Task CommitAsync()
        {
            await this.context.SaveChangesAsync();
        }

        protected override IRepository<TAggregateRoot> CreateRepository<TAggregateRoot>()
        {
            return new EFRepository<TAggregateRoot>(this);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (!disposed && this.context != null)
                {
                    this.context.Dispose();
                    this.disposed = true;
                }
            }
        }
    }
}
