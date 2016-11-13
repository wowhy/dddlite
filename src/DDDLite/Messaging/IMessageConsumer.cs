namespace DDDLite.Messaging
{
    using System;

    public interface IMessageConsumer : IDisposable
    {
        IMessageSubscriber Subscriber { get; }
    }
}