namespace DDDLite.WebApi.Exception
{
    using DDDLite.Exception;

    public class BadArgumentException : CoreException
    {
        private readonly string argument;

        public BadArgumentException(string argument) : this(argument, "用户输入参数错误")
        {
        }

        public BadArgumentException(string argument, string message) : base(message)
        {
            this.argument = argument;
        }

        public string Argument => this.argument;
    }
}