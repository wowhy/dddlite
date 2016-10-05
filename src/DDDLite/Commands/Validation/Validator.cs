namespace DDDLite.Commands.Validation
{
    using Commands;

    public abstract class Validator<TCommand> : IValidator
        where TCommand : ICommand
    {
        public virtual int Priority => 1;

        public void Validate(ICommand cmd)
        {
            this.DoValidate((TCommand)cmd);
        }

        public abstract void DoValidate(TCommand cmd);
    }
}
