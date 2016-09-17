namespace DDDLite.Application
{
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
