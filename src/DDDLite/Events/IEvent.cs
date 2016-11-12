namespace DDDLite.Events
{
    using System;

    public interface IEvent : IMessage
    {
        long Version { get; set; }
    }
}
