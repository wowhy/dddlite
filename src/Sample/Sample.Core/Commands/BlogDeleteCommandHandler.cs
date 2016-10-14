namespace Sample.Core.Commands
{
    using DDDLite.Commands;

    using Domain;
    using Repository;

    public class BlogDeleteCommandHandler : DeleteCommandHandler<BlogDeleteCommand, Blog>
    {
        public BlogDeleteCommandHandler(ISampleDomainRepositoryContext context) : base(context)
        {
        }
    }
}