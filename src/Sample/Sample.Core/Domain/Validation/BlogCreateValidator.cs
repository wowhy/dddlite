namespace Sample.Core.Domain.Validation
{
    using DDDLite.Commands.Validation;
    using DDDLite.CommandStack.Repository;

    using Commands;

    public class BlogCreateValidator : Validator<BlogCreateCommand>
    {
        private readonly IDomainRepository<Blog> repository;

        public BlogCreateValidator(IDomainRepository<Blog> repository)
        {
            this.repository = repository;
        }

        public override void DoValidate(BlogCreateCommand cmd)
        {
            if (cmd.Data.Title == "test")
            {
                throw new ValidationException("禁止创建测试数据");
            }
        }
    }
}