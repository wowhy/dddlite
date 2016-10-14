namespace DDDLite.Messaging
{
    using System;

    public interface IMessagePublisher : IDisposable
    {
        void Publish<TMessage>(TMessage message);
    }
}