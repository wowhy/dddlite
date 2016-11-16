namespace Sample.Core.Commands
{
    using DDDLite.Commands;
    using DDDLite.Commands.Validation;
    using DDDLite.Repository;
    using DDDLite.Repository.EntityFramework;
    using Entity;
    using Repository;
    using Validation;

    public class BlogCommandHandler : AggregateCommandHandler<Blog>
    {
        public BlogCommandHandler(IEFDomainRepository<Blog> repository) : base(repository)
        {
            this.AddValidator(typeof(CreateCommand<Blog>), new BlogCreateValidator(repository));
        }
    }
}
