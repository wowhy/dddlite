namespace Sample.Core.Domain.Commands
{
    using DDDLite.Commands;

    public class BlogUpdateCommand : UpdateCommand<Blog>
    {
        public BlogUpdateCommand()
        {
        }
    }
}