namespace DDDLite.WebApi.Models
{
    using System.Collections.Generic;

    public class ResponseValues<TData>
    {
        public List<TData> Value { get; set; }

        public int? Count { get; set; }
    }
}