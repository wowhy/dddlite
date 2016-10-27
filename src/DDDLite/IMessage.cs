namespace DDDLite
{
    using System;

    public interface IMessage
    {
        Guid Id { get; set; }

        DateTime Timestamp { get; set; }

        string GetTypeName();
    }
}