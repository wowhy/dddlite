﻿namespace DDDLite.Domain.Repositories
{
    using System;
    using DDDLite.Domain.Core;

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
    }
}