using System;

namespace DDDLite.Common
{
    public abstract class Message : IMessage
    {
        public Guid Id { get; set; }

        public DateTime Timestamp { get; set; }

        public Message()
        {
            this.Id = SequentialGuid.Create(SequentialGuidType.SequentialAsString);
            this.Timestamp = DateTime.Now;
        }

        public virtual string GetTypeName()
        {
            return this.GetType().FullName;
        }
    }
}