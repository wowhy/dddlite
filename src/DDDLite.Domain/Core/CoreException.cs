namespace DDDLite.Domain.Core
{
    using System;

    public class CoreException : Exception
    {
        public CoreException() { }

        public CoreException(string message)
            : base(message)
        { }

        public CoreException(string message, Exception innerException)
            : base(message, innerException)
        { }

        public CoreException(string format, params object[] args)
            : base(string.Format(format, args))
        { }
    }
}
