namespace DDDLite.Exception
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    public class AggregateRootException : CoreException
    {
        public AggregateRootException(Guid id, string message) : base(message)
        {
            this.Id = id;
        }

        public Guid Id { get; protected set; }
    }
}
