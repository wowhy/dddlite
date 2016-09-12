namespace Domain.Messaging
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public interface IMessageSubscriber : IDisposable
    {
        void Subscribe();

        event EventHandler<MessageReceivedEventArgs> MessageReceived;
    }
}
