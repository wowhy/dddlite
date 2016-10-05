namespace DDDLite.Commands.Validation
{
    using System.Collections.ObjectModel;

    public class ValidationCollection : Collection<IValidator>, IValidatorCollection
    {
    }
}
