namespace Domain.Core
{
    using System;
    using System.Runtime.Serialization;

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

        protected CoreException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        { }
    }
}
