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
        public BlogCommandHandler(ISampleDomainRepositoryContext context
            , BlogCreateValidator createValidaotr) : base(context)
        {
            this.AddValidator(typeof(BlogCreateCommand), new EntityValidator<Blog>());
            this.AddValidator(typeof(BlogCreateCommand), createValidaotr);
            this.AddValidator(typeof(BlogUpdateCommand), new EntityValidator<Blog>());
        }
    }
}
