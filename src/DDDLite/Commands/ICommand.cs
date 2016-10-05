namespace DDDLite.Commands
{
    using Validation;

    public interface ICommand
    {
        void Validate();

        IValidatorCollection Validators { get; }
    }
}
