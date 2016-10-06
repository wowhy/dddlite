namespace Sample.Core.CommandStack.Application
{
    using DDDLite.CommandStack.Application;
    
    using Domain;

    public interface IPostCommandService : IDomainCommandService<Post>
    {
    }
}