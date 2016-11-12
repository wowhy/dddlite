namespace DDDLite.Events
{
    using System;

    public abstract class Event : Message, IEvent
    {
        public long Version { get; set; }
    }
}
