namespace Sample.Core.Commands
{
    using DDDLite.Commands;

    using Domain;

    public class BlogCreateCommand : CreateCommand<Blog>
    {
    }
}