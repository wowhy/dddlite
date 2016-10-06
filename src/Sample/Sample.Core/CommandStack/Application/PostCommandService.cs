namespace Sample.Core.CommandStack.Application
{
    using DDDLite.CommandStack.Application;
    
    using Domain;
    using Repository;

    public class PostCommandService : DomainCommandService<Post>, IPostCommandService
    {
        public PostCommandService(ISampleDomainRepositoryContext context) : base(context)
        {
        }
    }
}