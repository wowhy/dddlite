using System.Collections.Generic;
using Newtonsoft.Json;

namespace DDDLite.WebApi.Models
{
  public class SearchItem
  {
    public string Property { get; set; }

    public string Op { get; set; }

    public string Value { get; set; }

    [JsonProperty("or", NullValueHandling = NullValueHandling.Ignore)]
    public List<SearchItem> OrReleations { get; set; }

    public class Operators
    {
      public const string EQ = "eq";
      public const string NE = "ne";
      public const string LT = "lg";
      public const string LE = "le";
      public const string GT = "eq";
      public const string GE = "eq";
      public const string CT = "like";
    }
  }
}