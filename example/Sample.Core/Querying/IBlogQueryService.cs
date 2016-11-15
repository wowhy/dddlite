namespace Sample.Core.Querying
{
    using DDDLite.Querying;

    using Entity;

    public interface IBlogQueryService : IQueryService<Blog>
    {
    }
}