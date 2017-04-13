using DDDLite.Repositories.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using DDDLite;

namespace Example.EF.Repositories
{
    public class ExampleDomainRepository<TAggregateRoot> : EFDomainRepository<TAggregateRoot>
        where TAggregateRoot : class, IAggregateRoot
    {
        public ExampleDomainRepository(WriteDbContext context) : base(context)
        {
        }
    }
}
