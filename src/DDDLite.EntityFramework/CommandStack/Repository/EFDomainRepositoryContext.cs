namespace DDDLite.EntityFramework.CommandStack.Repository
{
    using System.Threading.Tasks;

    using Microsoft.EntityFrameworkCore;

    using DDDLite.CommandStack.Repository;

    public class EFDomainRepositoryContext : DomainRepositoryContext, IEFDomainRepositoryContext
    {
        private readonly DbContext dbContext;

        public EFDomainRepositoryContext(DbContext context)
        {
            this.dbContext = context;
        }

        public DbContext DbContext => this.dbContext;

        public override void Commit()
        {
            this.dbContext.SaveChanges();
        }

        public override Task CommitAsync()
        {
            return this.dbContext.SaveChangesAsync();
        }

        public override IDomainRepository<TAggregateRoot> CreateRepository<TAggregateRoot>()
        {
            return new EFDomainRepository<TAggregateRoot>(this);
        }
    }
}