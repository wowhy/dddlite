namespace DDDLite.Commands
{
    using Common;
    using Validation;

    public interface ICommand : IMessage
    {
        void Validate();

        IValidatorCollection Validators { get; }
    }
}
