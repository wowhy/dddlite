namespace Sample.Core.Querying
{
    using DDDLite.Querying;

    using Domain;

    public interface IBlogQueryService : IQueryService<Blog>
    {
    }
}