namespace Sample.Core.QueryStack.Application
{
    using DDDLite.QueryStack.Application;

    using Domain;
    using Repository;

    public class BlogQueryService : QueryService<Blog>, IBlogQueryService
    {
        public BlogQueryService(ISampleQueryRepositoryContext context) : base(context)
        {
        }
    }
}