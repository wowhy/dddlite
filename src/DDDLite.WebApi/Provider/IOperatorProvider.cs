namespace DDDLite.WebApi.Provider
{
    using System;

    using DDDLite.Domain;

    public interface IOperatorProvider
    {
        Operator GetCurrentOperator();
    }
}