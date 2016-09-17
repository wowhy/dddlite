namespace DDDLite.Commands
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public interface ICommand
    {
        Guid Id { get; }

        DateTime Timestamp { get; }
    }
}
