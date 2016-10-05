namespace DDDLite
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;

    using AutoMapper;
    using AutoMapper.QueryableExtensions;

    public static class Extensions
    {
        public static IQueryable<T> Page<T>(this IQueryable<T> @this, int page = 1, int limit = 10)
        {
            return @this.Skip((page - 1) * limit).Take(limit);
        }

        public static PagedResult<T> AsPagedResult<T>(this IQueryable<T> @this, int page = 1, int limit = 10)
        {
            return new PagedResult<T>
            {
                Data = @this.Page(page, limit).ToList(),
                Total = @this.Count()
            };
        }


        public static bool TryValidate(this object @this)
        {
            return Extensions.TryValidate(@this, new List<ValidationResult>());
        }

        public static bool TryValidate(this object @this, ICollection<ValidationResult> validationErrors)
        {
            var context = new ValidationContext(@this, null, null);
            return Validator.TryValidateObject(@this, context, validationErrors, true);
        }

        #region Projections
        public static PagedResult<TDestination> ProjectToPagedResult<TDestination>(
            this IQueryable queryable,
            int page = 1,
            int limit = 10)
        {
            return queryable.ProjectTo<TDestination>().AsPagedResult(page, limit);
        }

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
