namespace DDDLite.WebApi.Models
{
    public class ResponseError
    {
        public ResponseError() { }

        public ResponseError(ErrorData error)
        {
            this.Error = error;
        }

        public ErrorData Error { get; set; }
    }
}