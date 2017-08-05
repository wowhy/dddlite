namespace DDDLite.WebApi.Models
{
    using System.Collections.Generic;
    
    public class ErrorData
    {
        public string Core { get; set; }

        public string Message { get; set; }

        public ErrorData InnerError { get; set; }

        public List<ErrorData> Details { get; set; }
    }
}