namespace DDDLite.WebApi.Internal.Parser
{
    using System;
    using System.Linq.Dynamic.Core;
    using System.Linq.Expressions;
    using DDDLite.Specifications;

    public class FilterParser<TAggregateRoot>
        where TAggregateRoot : class
    {
        public FilterParser()
        {
        }

        public Specification<TAggregateRoot> Parse(string filter)
        {
            var lambda = DynamicExpressionParser.ParseLambda(typeof(TAggregateRoot), typeof(bool), filter);
            var filterSpecification = Specification<TAggregateRoot>.Eval(lambda as Expression<Func<TAggregateRoot, bool>>);
            return filterSpecification;
        }
    }
}