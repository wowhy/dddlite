namespace DDDLite.Commands.Validation
{
    using System.ComponentModel.DataAnnotations;
    using System.Collections.Generic;
    using System.Linq;

    using Commands;
    using Domain;

    public class EntityValidator<TData> : IValidator
        where TData : class, IAggregateRoot
    {
        public int Priority => int.MaxValue;

        public void Validate(ICommand cmd)
        {
            var innerCmd = cmd as IDomainCommand<TData>;
            var entity = innerCmd.Data;

            var validationErrors = new List<ValidationResult>();
            var isValid = entity.TryValidate(validationErrors);
            if (!isValid)
            {
                throw new ValidationException("实体校验失败！", validationErrors.Select(k => k.ErrorMessage).ToArray());
            }
        }
    }
}
