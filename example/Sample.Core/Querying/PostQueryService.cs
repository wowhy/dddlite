namespace Sample.Core.Querying
{
    using DDDLite.Repository;
    using DDDLite.Querying;

    using Entity;
    using Repository;

    public class PostQueryService : QueryService<Post>, IPostQueryService
    {
        public PostQueryService(IQueryRepository<Post> repository) : base(repository)
        {
        }
    }
}