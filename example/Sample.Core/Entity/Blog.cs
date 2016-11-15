namespace Sample.Core.Entity
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using DDDLite;

    public class Blog : AggregateRoot
    {
        public Blog()
        {
            this.Posts = new List<Post>();
        }

        [Required]
        public string Title { get; set; }

        [Required]
        public string Url { get; set; }

        public virtual List<Post> Posts { get; set; }
    }
}