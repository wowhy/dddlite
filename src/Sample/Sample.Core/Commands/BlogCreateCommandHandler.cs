namespace Sample.Core.Commands
{
    using DDDLite.Commands;

    using Domain;
    using Repository;
    using Validation;

    public class BlogCreateCommandHandler : CreateCommandHandler<BlogCreateCommand, Blog>
    {
        public BlogCreateCommandHandler(ISampleDomainRepositoryContext context, BlogCreateValidator validator) : base(context, validator)
        {
        }
    }
}