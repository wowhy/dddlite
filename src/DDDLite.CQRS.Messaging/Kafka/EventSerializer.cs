namespace DDDLite.CQRS.Messaging.Kafka
{
  using System;
  using DDDLite.CQRS.Events;
  using MessagePack;

  public class EventSerializer : Confluent.Kafka.Serialization.ISerializer<IEvent>
  {
    public EventSerializer()
    {
    }

    public byte[] Serialize(IEvent data)
    {
      return MessagePackSerializer.Typeless.Serialize(data);
    }
  }
}