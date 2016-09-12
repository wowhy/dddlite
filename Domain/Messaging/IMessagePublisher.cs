namespace Domain.Messaging
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public interface IMessagePublisher : IDisposable
    {
        void Publish<TMessage>(TMessage message);
    }
}
