namespace Domain.Repositories
{
    using System;
    using System.Runtime.Serialization;
    using Domain.Core;

    public class RepositoryException : CoreException
    {
        public RepositoryException() { }

        public RepositoryException(string message)
            : base(message)
        { }

        public RepositoryException(string message, Exception innerException)
            : base(message, innerException)
        { }

        public RepositoryException(string format, params object[] args)
            : base(string.Format(format, args))
        { }

        protected RepositoryException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        { }
    }
}
