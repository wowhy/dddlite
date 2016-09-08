namespace Repository
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading;
    using System.Threading.Tasks;
    using Domain.Repository;
    using Microsoft.EntityFrameworkCore;

    public class EFUnitOfWork<TDbContext>
        where TDbContext : DbContext
    {
        public TDbContext Context { get; protected set; }
    }
}
