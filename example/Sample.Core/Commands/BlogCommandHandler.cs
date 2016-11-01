namespace Sample.Core.Commands
{
    using DDDLite.Commands;
    using DDDLite.Commands.Validation;
    using DDDLite.Repository;

    using Domain;
    using Repository;
    using Validation;

    public class BlogCommandHandler : AggregateCommandHandler<Blog>
    {
        public BlogCommandHandler(
            ISampleDomainRepositoryContext context, 
            BlogCreateValidator createValidator) : base(context)
        {
            this.AddValidator(typeof(CreateCommand<Blog>), createValidator);
        }
    }
}
