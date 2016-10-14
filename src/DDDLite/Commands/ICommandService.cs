namespace DDDLite.Commands
{
    using System;

    using Common;
    using Messaging;

    public interface ICommandService : IDisposable
    {
        IMessageSubscriber Subscriber { get; }

        IServiceProvider ServiceProvider { get; }
    }
}
