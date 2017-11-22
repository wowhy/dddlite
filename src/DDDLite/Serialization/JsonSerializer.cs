namespace DDDLite.Serialization
{
  using System;
  using System.Text;

  using Newtonsoft.Json;
  using Newtonsoft.Json.Serialization;

  public class JsonSerializer : IObjectSerializer
  {
    private readonly JsonSerializerSettings settings;
    private readonly Encoding encoding;

    public JsonSerializer()
    {
      this.encoding = Encoding.UTF8;
      this.settings = new JsonSerializerSettings
      {
        TypeNameHandling = TypeNameHandling.All,
        ContractResolver = new CamelCasePropertyNamesContractResolver()
      };
    }

    public JsonSerializer(JsonSerializerSettings settings, Encoding encoding)
    {
      this.settings = settings;
      this.encoding = encoding;
    }

    public T Deserialize<T>(byte[] buffer)
    {
      return JsonConvert.DeserializeObject<T>(encoding.GetString(buffer));
    }

    public byte[] Serialize<T>(T value)
    {
      return encoding.GetBytes(JsonConvert.SerializeObject(value, settings));
    }
  }
}