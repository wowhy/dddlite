namespace DDDLite.Specifications
{
    using System;
    using System.Linq.Expressions;

    using Domain;

    public class UndeletedSpecification<T> : Specification<T>
        where T : ILogicalDelete
    {
        public UndeletedSpecification()
        {
        }

        public override Expression<Func<T, bool>> Expression => k => k.Deleted == false;
    }
}
