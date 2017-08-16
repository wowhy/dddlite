namespace DDDLite.WebApi.Models
{
    using System.Collections.Generic;
    using Newtonsoft.Json;

    public class ErrorData
    {
        public string Code { get; set; }

        public string Message { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string Target { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public ErrorData InnerError { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public List<ErrorData> Details { get; set; }
    }
}