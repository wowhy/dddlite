namespace Sample.Core.Commands
{
    using DDDLite.Commands;

    using Domain;
    
    public class BlogDeleteCommand : DeleteCommand<Blog>
    {
        public BlogDeleteCommand()
        {
        }
    }
}