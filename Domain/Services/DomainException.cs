namespace Domain.Repositories
{
    using System;
    using System.Runtime.Serialization;
    using Domain.Core;

    public class DomainException : CoreException
    {
        public DomainException() { }

        public DomainException(string message)
            : base(message)
        { }

        public DomainException(string message, Exception innerException)
            : base(message, innerException)
        { }

        public DomainException(string format, params object[] args)
            : base(string.Format(format, args))
        { }

        protected DomainException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        { }
    }
}
