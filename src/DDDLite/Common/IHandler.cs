namespace DDDLite.Common
{
    using System.Threading.Tasks;
    
    public interface IHandler<in TMessage>
    {
        Task Handle(TMessage message);
    }
}