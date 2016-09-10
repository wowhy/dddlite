namespace Domain.Commands
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Core;

    public interface ICommandHandler
    {
    }

    public interface ICommandHandler<TCommand> : IHandler<TCommand>, ICommandHandler
        where TCommand : class, ICommand
    {
    }
}
