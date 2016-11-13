namespace DDDLite.Events
{
    public interface IEventHandler
    {
    }

    public interface IEventHandler<in TEvent> : IHandler<TEvent>, IEventHandler
    {
    }
}
