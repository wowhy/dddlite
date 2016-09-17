namespace DDDLite.Application
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Repositories;

    public class AppService : IAppService
    {
        private readonly IRepositoryContext context;

        public AppService(IRepositoryContext context)
        {
            this.context = context;
        }

        public IRepositoryContext Context => this.context;
    }
}
