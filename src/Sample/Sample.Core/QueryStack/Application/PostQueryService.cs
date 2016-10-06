namespace Sample.Core.QueryStack.Application
{
    using DDDLite.QueryStack.Application;

    using Domain;
    using Repository;

    public class PostQueryService : QueryService<Post>, IPostQueryService
    {
        public PostQueryService(ISampleQueryRepositoryContext context) : base(context)
        {
        }
    }
}