using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace DDDLite.Repositories.EF.UnitTests
{
    class SampleRepositoryContext : EFRepositoryContext
    {
        public SampleRepositoryContext() : base(new SampleDbContext())
        {
        }
    }
}
