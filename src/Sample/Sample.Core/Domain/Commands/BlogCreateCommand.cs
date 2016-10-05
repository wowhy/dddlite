namespace Sample.Core.Domain.Commands
{
    using DDDLite.Commands;
    using Validation;

    public class BlogCreateCommand : CreateCommand<Blog>
    {
        public BlogCreateCommand(BlogCreateValidator validator) : base(validator)
        {
        }
    }
}