namespace DDDLite.Commands.Validation
{
    using System;

    public class ValidationException : CoreException
    {
        private readonly string[] details;

        public string[] Details => this.details;

        public ValidationException()
        {
        }

        public ValidationException(string message)
            : base(message)
        {
        }

        public ValidationException(string message, string[] details)
            : base(message)
        {
            this.details = details;
        }

        public ValidationException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        public ValidationException(string message, string[] details, Exception innerException)
            : base(message, innerException)
        {
            this.details = details;
        }
    }
}
