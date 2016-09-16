namespace DDDLite.Domain.Events
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Core;

    public interface IEventHandler<in TEvent> : IHandler<TEvent>
        where TEvent : IEvent
    {
    }
}
