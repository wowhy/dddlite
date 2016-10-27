namespace DDDLite
{
    public interface IHandler<in TMessage>
    {
        void Handle(TMessage message);
    }
}