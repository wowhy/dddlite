namespace Sample.Core.Commands
{
    using DDDLite.Commands;

    using Domain;

    public class BlogUpdateCommand : UpdateCommand<Blog>
    {
        public BlogUpdateCommand()
        {
        }
    }
}