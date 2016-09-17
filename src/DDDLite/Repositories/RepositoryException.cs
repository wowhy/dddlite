namespace DDDLite.Domain.Repositories
{
    using System;
    using Core;

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
    }
}
