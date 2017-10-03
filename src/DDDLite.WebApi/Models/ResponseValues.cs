namespace DDDLite.WebApi.Models
{
    using System.Collections.Generic;
    using Newtonsoft.Json;

    public class ResponseValues<TData>
    {
        public ResponseValues() {}

        public ResponseValues(List<TData> value) 
        {
            this.Value = value;
        }

        public ResponseValues(List<TData> value, int count) 
        {
            this.Value = value;
            this.Count = count;
        }

        public List<TData> Value { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public int? Count { get; set; }
    }
}