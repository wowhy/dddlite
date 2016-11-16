namespace Sample.Core.Querying
{
    using DDDLite.Repository;
    using DDDLite.Querying;

    using Entity;
    using Repository;
    using DDDLite.Repository.EntityFramework;

    public class BlogQueryService : QueryService<Blog>, IBlogQueryService
    {
        public BlogQueryService(DDDLite.Repository.EntityFramework.IEFQueryRepository<Blog> repository) : base(repository)
        {
        }
    }
}