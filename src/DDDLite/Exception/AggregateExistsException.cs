namespace DDDLite.Exception
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    public class AggregateExistsException : CoreException
    {
        public AggregateExistsException(Guid id) : this(id, $"数据{id}重复")
        {
        }

        public AggregateExistsException(Guid id, string message) : base(message)
        {
            this.Id = id;
        }

        public Guid Id { get; private set; }
    }
}
