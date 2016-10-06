namespace Sample.Core.QueryStack.Application
{
    using DDDLite.QueryStack.Application;

    using Domain;

    public interface IPostQueryService : IQueryService<Post>
    {
    }
}