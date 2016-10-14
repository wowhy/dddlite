namespace Sample.Core.Querying
{
    using DDDLite.Querying;

    using Domain;
    using Repository;

    public class BlogQueryService : QueryService<Blog>, IBlogQueryService
    {
        public BlogQueryService(ISampleQueryRepositoryContext context) : base(context)
        {
        }
    }
}