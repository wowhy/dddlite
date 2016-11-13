namespace DDDLite
{
    using System;

    public interface IEntity
    {
        Guid Id { get; set; }

        void NewIdentity();
    }
}
