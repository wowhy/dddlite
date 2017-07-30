using Microsoft.EntityFrameworkCore;

namespace DDDLite.Repositories.EntityFramework
{
    public class RepositoryDbContext : DbContext
    {
		protected RepositoryDbContext()
        {
        }

        public RepositoryDbContext(DbContextOptions options) : base(options)
        {
        }
    }
}
