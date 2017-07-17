namespace DDDLite.Domain
{
    using System;
    
    public interface IAggregateRoot : IEntity, IConcurrencyToken
    {
    }
}