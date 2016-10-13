namespace DDDLite.Common
{
    using System.Threading.Tasks;
    
    public interface IHandler<in TMessage>
    {
        Task HandleAsync(TMessage message);
    }
}