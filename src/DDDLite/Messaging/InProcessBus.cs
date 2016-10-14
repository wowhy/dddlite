namespace DDDLite.Messaging
{
    using System;

    using Common;

    public class InProcessBus : DisposableObject, IMessagePublisher, IMessageSubscriber
    {
        private bool subscribed = false;

        public event EventHandler<MessageReceivedEventArgs> MessageReceived;

        public virtual void Publish<TMessage>(TMessage message)
        {
            if (subscribed)
            {
                this.OnMessageReceived(new MessageReceivedEventArgs(message));
            }
        }

        public virtual void Subscribe()
        {
            this.subscribed = true;
        }

        private void OnMessageReceived(MessageReceivedEventArgs e)
        {
            this.MessageReceived?.Invoke(this, e);
        }
    }
}
