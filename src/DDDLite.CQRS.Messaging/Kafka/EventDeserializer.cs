namespace DDDLite.CQRS.Messaging.Kafka
{
  using System;
  using System.Collections.Generic;
  using DDDLite.CQRS.Events;
  public class EventDeserializer : Confluent.Kafka.Serialization.IDeserializer<IEvent>
  {
    public IEnumerable<KeyValuePair<string, object>> Configure(IEnumerable<KeyValuePair<string, object>> config, bool isKey) => config;

    public IEvent Deserialize(string topic, byte[] data)
    {
      return MessagePack.MessagePackSerializer.Typeless.Deserialize(data) as IEvent;
    }
  }
}