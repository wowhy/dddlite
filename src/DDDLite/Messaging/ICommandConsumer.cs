namespace DDDLite.Messaging
{
    using System;
    
    public interface ICommandConsumer : IMessageConsumer
    {
        IServiceProvider ServiceProvider { get; }
    }
}
