namespace Sample.Core.CommandStack.Application
{
    using DDDLite.CommandStack.Application;
    
    using Domain;
    using Repository;

    public class BlogCommandService : DomainCommandService<Blog>, IBlogCommandService
    {
        public BlogCommandService(ISampleDomainRepositoryContext context) : base(context)
        {
        }
    }
}