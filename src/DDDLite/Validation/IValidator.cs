namespace DDDLite.Validation
{
    using Commands;

    public interface IValidator
    {
        void Validate(ICommand cmd);

        int Priority { get; }
    }
}
