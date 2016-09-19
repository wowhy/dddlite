namespace DDDLite.CommandStack.Application.Commands
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public interface ICommandHandler
    {
        void Handle<TCommand>() where TCommand : ICommand;
    }
}
