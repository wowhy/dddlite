namespace DDDLite.Commands.Validation
{
    using System.ComponentModel.DataAnnotations;
    using System.Collections.Generic;
    using System.Linq;

    using Commands;


    public class EntityValidator<TAggregateRoot> : Validation.Validator<IDomainCommand<TAggregateRoot>>
        where TAggregateRoot : class, IAggregateRoot
    {
        public override int Priority => int.MaxValue;

        public override void DoValidate(IDomainCommand<TAggregateRoot> cmd)
        {
            var entity = cmd.AggregateRoot;

            var validationErrors = new List<ValidationResult>();
            var isValid = entity.TryValidate(validationErrors);
            if (!isValid)
            {
                throw new ValidationException("实体校验失败！", validationErrors.Select(k => k.ErrorMessage).ToArray());
            }
        }
    }
}
