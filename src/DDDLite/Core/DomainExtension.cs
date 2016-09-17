namespace DDDLite.Core
{
    using System.Collections.Generic;
    using System.Linq;

    public static class DomainExtension
    {
        public static IEnumerable<TAggregateRoot> Page<TAggregateRoot>(this IQueryable<TAggregateRoot> @this, int page = 1, int limit = 10)
            where TAggregateRoot : class, IAggregateRoot
        {
            return @this.Skip((page - 1) * limit).Take(limit);
        }
    }
}
