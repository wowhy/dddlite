namespace DDDLite.CQRS.Messaging.Kafka
{
  public class KafkaEventBusOptions
  {
    public string Host { get; set; }

    public string GroupId { get; set; }

    public string PublishToptic { get; set; }

    public string[] SubscribeToptics { get; set; }
  }
}