namespace DDDLite.WebApi.Models
{
    using System.Collections.Generic;
    using Newtonsoft.Json;

    public class ResponseValues<TData>
    {
        public List<TData> Value { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public int? Count { get; set; }
    }
}