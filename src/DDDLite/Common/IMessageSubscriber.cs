namespace DDDLite.Common
{
    using System;

    public interface IMessageSubscriber : IDisposable
    {
        void Subscribe();

        event EventHandler<MessageReceivedEventArgs> MessageReceived;
    }
}