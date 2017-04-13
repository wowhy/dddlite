using System.Collections.Generic;
using System.Linq.Expressions;

namespace DDDLite.Querying
{
    public class Filter
    {
        public Filter()
        {
        }

        public Filter(string prop, object value)
        {
            this.Property = prop;
            this.Value = value;
            this.Operator = Operators.Equal;
        }

        public Filter(string prop, object value, string op)
        {
            this.Property = prop;
            this.Value = value;
            this.Operator = op;
        }

        public string Property { get; set; }
        public string Operator { get; set; }
        public object Value { get; set; }

        public ICollection<Filter> OrConnectedFilters { get; set; } = new List<Filter>();

        public class Operators
        {
            public const string GreaterThan = ">";
            public const string GreaterThanOrEqual = ">=";
            public const string LessThan = "<";
            public const string LessThanOrEqual = "<=";
            public const string Equal = "=";
            public const string NotEqual = "!=";
            public const string StartsWith = "~";
            public const string NotStartsWith = "!~";
            public const string Contains = "*";
            public const string NotContains = "!*";

            //public const string Any = "in";
            //public const string NotAny = "not in";

            public static Expression GetExpression(string op, Expression left, Expression right)
            {
                switch (op)
                {
                    case GreaterThan:
                        return Expression.GreaterThan(left, right);
                    case GreaterThanOrEqual:
                        return Expression.GreaterThanOrEqual(left, right);
                    case LessThan:
                        return Expression.LessThan(left, right);
                    case LessThanOrEqual:
                        return Expression.LessThanOrEqual(left, right);
                    case Equal:
                        return Expression.Equal(left, right);
                    case NotEqual:
                        return Expression.NotEqual(left, right);
                    case StartsWith:
                        return Expression.Call(left, "StartsWith", null, right);
                    case NotStartsWith:
                        return Expression.Not(Expression.Call(left, "StartsWith", null, right));
                    case Contains:
                        return Expression.Call(left, "Contains", null, right);
                    case NotContains:
                        return Expression.Not(Expression.Call(left, "Contains", null, right));
                }

                return Expression.Equal(left, right);
            }
        }
    }
}