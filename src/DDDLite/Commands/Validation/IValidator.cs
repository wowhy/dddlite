namespace DDDLite.Commands.Validation
{
    using Commands;

    public interface IValidator
    {
        void Validate(ICommand cmd);

        int Priority { get; }
    }
}
