namespace DDDLite.Exception
{
    using System;

    public class ConcurrencyException : CoreException
    {
        public ConcurrencyException(Exception innerException) : base("数据版本不一致", innerException)
        {
        }

        public ConcurrencyException(string message, Exception innerException) : base(message, innerException) {}
    }
}
