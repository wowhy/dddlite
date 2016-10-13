namespace DDDLite.Commands
{
    using System;
    using Common;

    public class InProcessCommandBus : DisposableObject, ICommandSender, IMessageSubscriber
    {
        private bool subscribed = false;

        public event EventHandler<MessageReceivedEventArgs> MessageReceived;

        public void Publish<TMessage>(TMessage message)
        {
            if (subscribed)
            {
                this.OnMessageReceived(new MessageReceivedEventArgs(message));
            }
        }

        public void Subscribe()
        {
            this.subscribed = true;
        }

        private void OnMessageReceived(MessageReceivedEventArgs e)
        {
            this.MessageReceived?.Invoke(this, e);
        }
    }
}
