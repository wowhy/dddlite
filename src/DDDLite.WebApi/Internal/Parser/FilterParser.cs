namespace DDDLite.WebApi
{
    using System;
    using DDDLite.Specifications;

    public class FilterParser<TAggregateRoot>
        where TAggregateRoot: class
    {
        public FilterParser()
        {
        }

        public Specification<TAggregateRoot> Parse(string filter)
        {
            var filterSpecification = Specification<TAggregateRoot>.Any();
            return filterSpecification;
        }
    }
}