namespace DDDLite.Validation
{
    using System.ComponentModel.DataAnnotations;
    using System.Collections.Generic;
    using System.Linq;

    using Commands;


    public class EntityValidator : Validator<IAggregateRootCommand>
    {
        public override int Priority => int.MaxValue;

        public override void DoValidate(IAggregateRootCommand cmd)
        {
            var entity = cmd.AggregateRoot;

            var validationErrors = new List<ValidationResult>();
            var isValid = entity.TryValidate(validationErrors);
            if (!isValid)
            {
                throw new CoreValidateException("实体校验失败！", validationErrors.Select(k => k.ErrorMessage).ToArray());
            }
        }
    }
}
