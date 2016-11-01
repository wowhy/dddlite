namespace Sample.Core.Commands.Validation
{
    using DDDLite.Commands;
    using DDDLite.Commands.Validation;
    using DDDLite.Repository;

    using Domain;

    public class BlogCreateValidator : Validator<CreateCommand<Blog>>
    {
        private readonly IDomainRepository<Blog> repository;

        public BlogCreateValidator(IDomainRepository<Blog> repository)
        {
            this.repository = repository;
        }

        public override void DoValidate(CreateCommand<Blog> cmd)
        {
            if (cmd.AggregateRoot.Title == "test")
            {
                throw new ValidationException("禁止创建测试数据");
            }
        }
    }
}