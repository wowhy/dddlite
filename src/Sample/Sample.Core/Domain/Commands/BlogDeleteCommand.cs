namespace Sample.Core.Domain.Commands
{
    using DDDLite.Commands;

    public class BlogDeleteCommand : DeleteCommand<Blog>
    {
        public BlogDeleteCommand()
        {
        }
    }
}