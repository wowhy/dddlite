namespace DDDLite.Core
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using AutoMapper;
    using AutoMapper.QueryableExtensions;

    public static class DomainExtension
    {
        public static IEnumerable<TAggregateRoot> Page<TAggregateRoot>(this IQueryable<TAggregateRoot> @this, int page = 1, int limit = 10)
            where TAggregateRoot : class, IAggregateRoot
        {
            return @this.Skip((page - 1) * limit).Take(limit);
        }

        #region Projections
        public static List<TDestination> ProjectToList<TDestination>(this IQueryable queryable, IConfigurationProvider config)
        {
            return queryable.ProjectTo<TDestination>(config).ToList();
        }

        public static List<TDestination> ProjectToList<TDestination>(this IQueryable queryable)
        {
            return queryable.ProjectTo<TDestination>().ToList();
        }

        public static TDestination[] ProjectToArray<TDestination>(this IQueryable queryable, IConfigurationProvider config)
        {
            return queryable.ProjectTo<TDestination>(config).ToArray();
        }

        public static TDestination[] ProjectToArray<TDestination>(this IQueryable queryable)
        {
            return queryable.ProjectTo<TDestination>().ToArray();
        }

        public static TDestination ProjectToSingleOrDefault<TDestination>(this IQueryable queryable, IConfigurationProvider config)
        {
            return queryable.ProjectTo<TDestination>(config).SingleOrDefault();
        }

        public static TDestination ProjectToSingleOrDefault<TDestination>(this IQueryable queryable)
        {
            return queryable.ProjectTo<TDestination>().SingleOrDefault();
        }

        public static TDestination ProjectToSingle<TDestination>(this IQueryable queryable, IConfigurationProvider config)
        {
            return queryable.ProjectTo<TDestination>(config).Single();
        }

        public static TDestination ProjectToSingle<TDestination>(this IQueryable queryable)
        {
            return queryable.ProjectTo<TDestination>().Single();
        }

        public static TDestination ProjectToFirstOrDefault<TDestination>(this IQueryable queryable, IConfigurationProvider config)
        {
            return queryable.ProjectTo<TDestination>(config).FirstOrDefault();
        }

        public static TDestination ProjectToFirstOrDefault<TDestination>(this IQueryable queryable)
        {
            return queryable.ProjectTo<TDestination>().FirstOrDefault();
        }

        public static TDestination ProjectToFirst<TDestination>(this IQueryable queryable, IConfigurationProvider config)
        {
            return queryable.ProjectTo<TDestination>(config).First();
        }

        public static TDestination ProjectToFirst<TDestination>(this IQueryable queryable)
        {
            return queryable.ProjectTo<TDestination>().First();
        }

        public static IQueryable<TDestination> ProjectToQueryable<TDestination>(this IQueryable queryable, IConfigurationProvider config)
        {
            return queryable.ProjectTo<TDestination>(config);
        }

        public static IQueryable<TDestination> ProjectToQueryable<TDestination>(this IQueryable queryable)
        {
            return queryable.ProjectTo<TDestination>();
        }

        #endregion
    }
}
