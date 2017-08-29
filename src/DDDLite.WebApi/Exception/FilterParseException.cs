namespace DDDLite.WebApi.Exception
{
    using System;
    using DDDLite.Exception;

    public class FilterParseException : CoreException
    {
        public FilterParseException(Exception innerException) : base(innerException.Message, innerException)
        {
        }

        public FilterParseException(int index)
            : base($"FilterParseException at string index {index}")
        {
        }
    }
}