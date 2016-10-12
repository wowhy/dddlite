namespace DDDLite.Commands
{
    using System;

    using Common;

    public interface ICommandService : IDisposable
    {
        IMessageSubscriber Subscriber { get; }

        IServiceProvider ServiceProvider { get; }
    }
}
