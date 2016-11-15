namespace Sample.Core.Entity
{
    using System;

    using DDDLite;

    public class Post : AggregateRoot
    {
        public string Title { get; set; }

        public string Content { get; set; }

        public Guid BlogId { get; set; }

        public virtual Blog Blog { get; set; }
    }
}