namespace DDDLite.Specifications
{
    using DDDLite.Domain;
    using DDDLite.Querying;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;

    /// <summary>
    /// Represents the sort specification in a query that is used for defining the sort field and order.
    /// </summary>
    /// <typeparam name="TKey">The type of the aggregate root key.</typeparam>
    /// <typeparam name="T">The type of the aggregate root.</typeparam>
    public sealed class SortSpecification<T> : IDictionary<string, SortDirection>
    {
        #region Nested Internal Classes
        private class DumpMemberAccessNameVisitor : ExpressionVisitor
        {
            private List<string> nameList = new List<string>();
            protected override Expression VisitMember(MemberExpression node)
            {
                var expression = base.VisitMember(node);
                nameList.Add(node.Member.Name);
                return expression;
            }

            public string MemberAccessName => string.Join(".", nameList);

            public override string ToString() => MemberAccessName;
        }
        #endregion 

        private readonly Dictionary<string, SortDirection> sortSpecifications = new Dictionary<string, SortDirection>();

        public static readonly SortSpecification<T> None = new SortSpecification<T>();

        public static readonly SortSpecification<T> SortById = new SortSpecification<T>(nameof(IAggregateRoot.Id));

        public static readonly SortSpecification<T> SortByCreatedAtAsc = new SortSpecification<T>(nameof(IAggregateRoot.CreatedAt));

        public static readonly SortSpecification<T> SortByCreatedAtDesc = new SortSpecification<T>(nameof(IAggregateRoot.CreatedAt), SortDirection.Desc);

        public SortSpecification()
        {
        }

        public SortSpecification(string key)
        {
            this.Add(key, SortDirection.Asc);
        }

        public SortSpecification(string key, SortDirection dir)
        {
            this.Add(key, dir);
        }

        public SortDirection this[string key]
        {
            get
            {
                return sortSpecifications[key];
            }

            set
            {
                sortSpecifications[key] = value;
            }
        }

        public int Count
        {
            get
            {
                return sortSpecifications.Count;
            }
        }

        public bool IsReadOnly
        {
            get
            {
                return false;
            }
        }

        public ICollection<string> Keys
        {
            get
            {
                return sortSpecifications.Keys;
            }
        }

        public ICollection<SortDirection> Values
        {
            get
            {
                return sortSpecifications.Values;
            }
        }

        private static Expression<Func<T, object>> CreateLambdaExpression(string propertyName)
        {
            var param = Expression.Parameter(typeof(T), "x");
            Expression body = param;
            foreach (var member in propertyName.Split('.'))
            {
                body = Expression.Property(body, member);
            }
            return Expression.Lambda<Func<T, object>>(Expression.Convert(body, typeof(object)), param);
        }

        public void Add(KeyValuePair<string, SortDirection> item)
        {
            Add(item.Key, item.Value);
        }

        public void Add(string key, SortDirection value)
        {
            sortSpecifications.Add(key, value);
        }

        public void Add(Expression<Func<T, object>> sortExpression, SortDirection dir)
        {
            var visitor = new DumpMemberAccessNameVisitor();
            visitor.Visit(sortExpression);
            var memberAccessName = visitor.MemberAccessName;
            if (!ContainsKey(memberAccessName))
            {
                Add(memberAccessName, dir);
            }
        }

        public IEnumerable<Tuple<Expression<Func<T, object>>, SortDirection>> Specifications
        {
            get
            {
                foreach (var kvp in sortSpecifications)
                {
                    yield return new Tuple<Expression<Func<T, object>>, SortDirection>(CreateLambdaExpression(kvp.Key), kvp.Value);
                }
            }
        }

        public void Clear()
        {
            sortSpecifications.Clear();
        }

        public bool Contains(KeyValuePair<string, SortDirection> item)
        {
            return sortSpecifications.Contains(item);
        }

        public bool ContainsKey(string key)
        {
            return sortSpecifications.ContainsKey(key);
        }

        public void CopyTo(KeyValuePair<string, SortDirection>[] array, int arrayIndex)
        {
            ((ICollection)sortSpecifications).CopyTo(array, arrayIndex);
        }

        public IEnumerator<KeyValuePair<string, SortDirection>> GetEnumerator()
        {
            return sortSpecifications.GetEnumerator();
        }

        public bool Remove(KeyValuePair<string, SortDirection> item)
        {
            return sortSpecifications.Remove(item.Key);
        }

        public bool Remove(string key)
        {
            return sortSpecifications.Remove(key);
        }

        public bool TryGetValue(string key, out SortDirection value)
        {
            return sortSpecifications.TryGetValue(key, out value);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return sortSpecifications.GetEnumerator();
        }
    }
}
