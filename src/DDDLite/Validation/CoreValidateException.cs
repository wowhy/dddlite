namespace DDDLite.Validation
{
    using System;

    public class CoreValidateException : CoreException
    {
        private readonly string[] details;

        public string[] Details => this.details;

        public CoreValidateException()
        {
        }

        public CoreValidateException(string message)
            : base(message)
        {
        }

        public CoreValidateException(string message, string[] details)
            : base(message)
        {
            this.details = details;
        }

        public CoreValidateException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        public CoreValidateException(string message, string[] details, Exception innerException)
            : base(message, innerException)
        {
            this.details = details;
        }
    }
}
