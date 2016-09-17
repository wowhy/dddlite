namespace DDDLite.Events
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public interface IEvent
    {
        Guid Id { get; }

        DateTime Timestamp { get; }
    }
}
