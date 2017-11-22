namespace DDDLite.CQRS.Messaging.Kafka
{
  using System;
  using DDDLite.CQRS.Events;
  public class EventDeserializer : Confluent.Kafka.Serialization.IDeserializer<IEvent>
  {
    public IEvent Deserialize(byte[] data)
    {
      return MessagePack.MessagePackSerializer.Typeless.Deserialize(data) as IEvent;
    }
  }
}