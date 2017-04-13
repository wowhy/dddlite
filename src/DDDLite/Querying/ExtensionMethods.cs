namespace DDDLite.Querying
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Specifications;
    using System.Linq.Expressions;
    using System.ComponentModel;
    using System.Reflection;

    public static class ExtensionMethods
    {
        public static Specification<T> ToSpecification<T>(this ICollection<Filter> filters)
        {
            if (filters == null || filters.Count == 0)
            {
                return Specification<T>.Any();
            }

            return new ExpressionSpecification<T>(ToExpression<T>(filters));
        }

        public static SortSpecification<T> ToSpecification<T>(this ICollection<Sorter> sorters)
        {
            if (sorters == null || sorters.Count == 0)
            {
                return SortSpecification<T>.None;
            }

            var spec = new SortSpecification<T>();
            foreach (var sorter in sorters)
            {
                spec[sorter.Property] = sorter.SortOrder;
            }

            return spec;
        }

        public static Expression<Func<T, bool>> ToExpression<T>(this ICollection<Filter> filters)
        {
            var expr = Specification<T>.Any().Expression;
            if (filters == null || filters.Count == 0)
            {
                return expr;
            }

            foreach (var filter in filters)
            {
                var right = filter.ToExpression<T>();
                expr = ExpressionFuncExtender.And(expr, right);
            }
            return expr;
        }

        public static Expression<Func<T, bool>> ToExpression<T>(this Filter filter)
        {
            var expr = Specification<T>.Any().Expression;
            if (filter == null)
            {
                return expr;
            }

            expr = Translate<T>(filter);

            if (filter.OrConnectedFilters != null)
            {
                foreach (var orFilter in filter.OrConnectedFilters)
                {
                    var right = orFilter.ToExpression<T>();
                    expr = ExpressionFuncExtender.Or(expr, right);
                }
            }

            return expr;
        }

        #region Privates
        private static readonly Type StringType = typeof(string);
        private static readonly Type[] AvailableCastTypes =
        {
            typeof(DateTime),
            typeof(DateTime?),
            typeof(TimeSpan),
            typeof(TimeSpan?),
            typeof(bool),
            typeof(bool?),
            typeof(int),
            typeof(int?),
            typeof(uint),
            typeof(uint?),
            typeof(long),
            typeof(long?),
            typeof(ulong),
            typeof(ulong?),
            typeof(Guid),
            typeof(Guid?),
            typeof(double),
            typeof(double?),
            typeof(float),
            typeof(float?),
            typeof(decimal),
            typeof(decimal?),
            typeof(char),
            typeof(char?),
            typeof(string)
        };
        private static object CastFieldValue(object value, Type type)
        {
            if (value.GetType() == type)
            {
                return value;
            }

            if (type.IsEnum)
            {
                if (value.GetType() != typeof(string))
                {
                    return Enum.ToObject(type, value);
                }
            }

            var typeConverter = TypeDescriptor.GetConverter(type);
            return typeConverter.ConvertFrom(value);
        }
        private static Expression<Func<T, bool>> Translate<T>(Filter filter)
        {
            if (string.IsNullOrEmpty(filter.Property))
            {
                throw new ArgumentNullException(nameof(filter.Property));
            }

            var p = Expression.Parameter(typeof(T), "p");
            var parts = filter.Property.Split('.');
            var whereProperty = parts.Aggregate<string, Expression>(p, Expression.Property);
            var constant = default(Expression);

            if (filter.Value != null)
            {
                constant = Expression.Constant(CastFieldValue(filter.Value, whereProperty.Type), whereProperty.Type);
            }
            else
            {
                if (whereProperty.Type.IsValueType)
                {
                    if (!whereProperty.Type.IsGenericType || whereProperty.Type.GetGenericTypeDefinition() != typeof(Nullable<>))
                    {

                        whereProperty = Expression.MakeUnary(
                            ExpressionType.Convert,
                            whereProperty,
                            typeof(Nullable<>).MakeGenericType(whereProperty.Type));
                    }
                }

                constant = Expression.Constant(null, whereProperty.Type);
            }

            var exprBody = Filter.Operators.GetExpression(filter.Operator, whereProperty, constant);

            return Expression.Lambda<Func<T, bool>>(exprBody, p);
        }
        #endregion
    }
}