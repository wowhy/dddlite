namespace Sample.Core.Querying
{
    using DDDLite.Repository;
    using DDDLite.Querying;

    using Entity;
    using Repository;

    public class BlogQueryService : QueryService<Blog>, IBlogQueryService
    {
        public BlogQueryService(IQueryRepository<Blog> repository) : base(repository)
        {
        }
    }
}