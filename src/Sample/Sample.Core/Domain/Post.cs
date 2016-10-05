namespace Sample.Core.Domain
{
    using System;

    using DDDLite.Domain;

    public class Post : AggregateRoot
    {
        public string Title { get; set; }

        public string Content { get; set; }

        public Guid BlogId { get; set; }

        public virtual Blog Blog { get; set; }
    }
}