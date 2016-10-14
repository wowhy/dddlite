namespace Sample.Core.Commands
{
    using DDDLite.Commands;

    using Domain;
    using Repository;

    public class BlogUpdateCommandHandler : UpdateCommandHandler<BlogUpdateCommand, Blog>
    {
        public BlogUpdateCommandHandler(ISampleDomainRepositoryContext context) : base(context)
        {
        }
    }
}