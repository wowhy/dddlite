namespace DDDLite.CQRS.Messaging.Kafka
{
  using System;
  using System.Collections.Generic;
  using DDDLite.CQRS.Events;
  using MessagePack;

  public class EventSerializer : Confluent.Kafka.Serialization.ISerializer<IEvent>
  {
    public EventSerializer()
    {
    }

    public IEnumerable<KeyValuePair<string, object>> Configure(IEnumerable<KeyValuePair<string, object>> config, bool isKey) => config;

    public byte[] Serialize(string topic, IEvent data)
    {
      if (data == null)
      {
        return null;
      }

      return MessagePackSerializer.Typeless.Serialize(data);
    }
  }
}