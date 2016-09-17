namespace DDDLite.Domain.Core
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public interface IHandler<in TMessage>
    {
        Task HandleAsync(TMessage message);

        void Handle(TMessage message);
    }
}
