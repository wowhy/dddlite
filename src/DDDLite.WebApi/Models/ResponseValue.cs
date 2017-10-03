namespace DDDLite.WebApi.Models
{
    public class ResponseValue<TData>
    {
        public ResponseValue() 
        {}

        public ResponseValue(TData value) 
        {
            this.Value = value;
        }

        public TData Value { get; set; }
    }
}