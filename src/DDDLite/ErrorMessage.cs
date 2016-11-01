namespace DDDLite
{
    using System.Collections.Generic;

    public class ErrorMessage
    {
        public string Message { get; set; }

        public List<string> Details { get; set; }
        
        public ErrorMessage()
        {
        }

        public ErrorMessage(string message, IEnumerable<string> details) 
        {
            this.Message = message;
            this.Details = new List<string>();
            if(details != null)
            {
                this.Details.AddRange(details);
            }
        }
    }
}