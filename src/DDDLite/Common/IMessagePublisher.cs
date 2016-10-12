namespace DDDLite.Common
{
    using System;

    public interface IMessagePublisher : IDisposable
    {
        void Publish<TMessage>(TMessage message);
    }
}