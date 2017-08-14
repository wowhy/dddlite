namespace DDDLite.WebApi.Internal.Parser
{
    using System;
    using System.Linq.Dynamic.Core;
    using System.Linq.Expressions;
    using DDDLite.Specifications;
    using DDDLite.WebApi.Exception;

    public class FilterParser<TAggregateRoot>
        where TAggregateRoot : class
    {
        public FilterParser()
        {
        }

        public Specification<TAggregateRoot> Parse(string filter)
        {
            try
            {
                // TODO: 使用自定义语法解析
                // 动态lambda在使用中比较复杂难以学习 

                var lambda = DynamicExpressionParser.ParseLambda(typeof(TAggregateRoot), typeof(bool), filter);
                var filterSpecification = Specification<TAggregateRoot>.Eval(lambda as Expression<Func<TAggregateRoot, bool>>);
                return filterSpecification;
            }
            catch (Exception)
            {
                throw new FilterParseException();
            }
        }
    }
}