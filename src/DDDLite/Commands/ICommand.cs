namespace DDDLite.Commands
{
    using System;
    
    public interface ICommand : IMessage
    {
        Guid OperatorId { get; set; }
    }
}
