namespace DDDLite.Commands
{
    using Validation;
    using System.Linq;

    public abstract class Command : ICommand
    {
        private readonly ValidationCollection validators = new ValidationCollection();

        protected Command()
        { }

        protected Command(params IValidator[] validators)
        {
            foreach (var validator in validators)
            {
                this.validators.Add(validator);
            }
        }

        public IValidatorCollection Validators => this.validators;

        public void Validate()
        {
            foreach (var validator in validators.OrderByDescending(k => k.Priority))
            {
                validator.Validate(this);
            }
        }
    }
}
