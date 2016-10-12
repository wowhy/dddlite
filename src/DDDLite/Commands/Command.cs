namespace DDDLite.Commands
{
    using Common;

    public abstract class Command : Message, ICommand
    {
        protected Command()
        { }
    }
}
