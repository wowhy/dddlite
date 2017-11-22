namespace DDDLite.Serialization
{
  using Serializer = MessagePack.MessagePackSerializer.Typeless;

  public class MsgPackSerializer : IObjectSerializer
  {
    public T Deserialize<T>(byte[] buffer)
    {
      return (T)Serializer.Deserialize(buffer);
    }

    public byte[] Serialize<T>(T value)
    {
      return Serializer.Serialize(value);
    }
  }
}