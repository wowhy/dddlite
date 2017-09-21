namespace DDDLite.WebApi.Exception
{
    using DDDLite.Exception;

    public class BadArgumentException : CoreException
    {
        private readonly string argument;

        public BadArgumentException(string argument) : this(argument, string.Empty)
        {
        }

        public BadArgumentException(string argument, string message) : base(message)
        {
            this.argument = argument;
        }

        public string Argument => this.argument;
    }
}