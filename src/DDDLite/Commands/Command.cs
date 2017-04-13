namespace DDDLite.Commands
{
    using System;

    public abstract class Command : Message, ICommand
    {
        protected Command()
        { }

        public Guid? OperatorId { get; set; }

        public string OperatorName { get; set; }
    }
}
