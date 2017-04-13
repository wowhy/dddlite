namespace DDDLite.Querying.EF
{
    using Microsoft.EntityFrameworkCore;
    using System.Linq;

    public interface IEFQueryService<TReadModel> : IQueryService<TReadModel>
        where TReadModel : class, new()
    {
        DbContext Context { get; }

        IQueryable<TReadModel> NoTrackingSet(string op, string[] eagerLoadings);
    }
}
