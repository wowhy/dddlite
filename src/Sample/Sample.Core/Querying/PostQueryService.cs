namespace Sample.Core.Querying
{
    using DDDLite.Querying;

    using Domain;
    using Repository;

    public class PostQueryService : QueryService<Post>, IPostQueryService
    {
        public PostQueryService(ISampleQueryRepositoryContext context) : base(context)
        {
        }
    }
}