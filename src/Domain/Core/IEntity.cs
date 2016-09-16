namespace Domain.Core
{
    using System;

    public interface IEntity
    {
        Guid Id { get; set; }
    }
}
