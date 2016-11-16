namespace Sample.Core.Querying
{
    using DDDLite.Repository;
    using DDDLite.Querying;

    using Entity;
    using Repository;
    using DDDLite.Repository.EntityFramework;

    public class PostQueryService : QueryService<Post>, IPostQueryService
    {
        public PostQueryService(IEFQueryRepository<Post> repository) : base(repository)
        {
        }
    }
}