namespace DDDLite.Exception
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    public class AggregateNotFoundException : CoreException
    {
        public AggregateNotFoundException(Guid id) : this(id, $"数据{id}不存在") 
        {

        }

        public AggregateNotFoundException(Guid id, string message) : base(message)
        {
            this.Id = id;
        }

        public Guid Id { get; private set; }
    }
}
