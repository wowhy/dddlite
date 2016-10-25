namespace DDDLite.Querying
{
    using System.Collections.Generic;
    using Specifications;

    public static class Extensions
    {
        public static Specification<T> ToSpecification<T>(this ICollection<Filter> filters)
        {
            if (filters == null || filters.Count == 0)
            {
                return Specification<T>.Any();
            }

            // TODO: 翻译sorters
            return Specification<T>.Any();
        }

        public static SortSpecification<T> ToSpecification<T>(this ICollection<Sorter> sorters)
        {
            if (sorters == null || sorters.Count == 0)
            {
                return SortSpecification<T>.None;
            }

            // TODO: 翻译sorters
            return SortSpecification<T>.None;
        }
    }
}