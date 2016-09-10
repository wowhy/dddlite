namespace Domain.Commands
{
    using System;
    using Domain.Core;

    public interface ICommand
    {
        Guid Id { get; set; }
    }
}
