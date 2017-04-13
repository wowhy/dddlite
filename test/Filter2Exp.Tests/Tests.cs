using DDDLite.Querying;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Xunit;
namespace Tests
{
    public class Tests
    {
        public class User
        {
            public Guid Id { get; set; }
            public string Code { get; set; }

            public int Number1 { get; set; }
            public double Number2 { get; set; }
            public decimal Number3 { get; set; }

            public DateTime Time { get; set; }

            public MoreInfo More { get; set; }
        }

        public class MoreInfo
        {
            public int? Age { get; set; }
            public DateTime? Birthday { get; set; }
        }

        [Fact]
        public void TestGuid()
        {
            var filters = new List<Filter>()
            {
                new Filter
                {
                    Property = "Id",
                    Operator = Filter.Operators.Equal,
                    Value = null
                },
                new Filter
                {
                    Property = "Id",
                    Operator = Filter.Operators.Equal,
                    Value = Guid.NewGuid()
                },
                new Filter
                {
                    Property = "Id",
                    Operator = Filter.Operators.Equal,
                    Value = Guid.NewGuid().ToString()
                }
            };

            var expr = filters.ToExpression<User>();

            Assert.True(true);
        }

        [Fact]
        public void TestString()
        {
            var filters = new List<Filter>()
            {
                new Filter
                {
                    Property = "Code",
                    Operator = Filter.Operators.Equal,
                    Value = null
                },
                new Filter
                {
                    Property = "Code",
                    Operator = Filter.Operators.Contains,
                    Value = "321"
                },
                new Filter
                {
                    Property = "Code",
                    Operator = Filter.Operators.NotStartsWith,
                    Value = "123"
                },
                new Filter
                {
                    Property = "Code",
                    Operator = Filter.Operators.StartsWith,
                    Value = null
                }
            };

            filters.ToExpression<User>();

            Assert.True(true);
        }

        [Fact]
        public void TestPath()
        {
            var filters = new List<Filter>()
            {
                new Filter
                {
                    Property = "More.Age",
                    Operator = Filter.Operators.Equal,
                    Value = null
                },
                new Filter
                {
                    Property = "More.Age",
                    Operator = Filter.Operators.GreaterThanOrEqual,
                    Value = "321"
                },
                new Filter
                {
                    Property = "More.Birthday",
                    Operator = Filter.Operators.GreaterThan,
                    Value = DateTime.Now
                },
                new Filter
                {
                    Property = "More.Birthday",
                    Operator = Filter.Operators.LessThan,
                    Value = DateTime.Now.ToString()
                }
            };

            filters.ToExpression<User>();

            Assert.True(true);
        }
    }
}
