using DDDLite.Exception;

namespace DDDLite.WebApi.Exception
{
    public class SorterParseException : CoreException
    {
        public SorterParseException() : base("无法解析$sorter参数")
        {
        }

        public SorterParseException(string message) : base(message)
        {
        }

        public SorterParseException(string message, System.Exception innerException) : base(message, innerException)
        {
        }
    }
}