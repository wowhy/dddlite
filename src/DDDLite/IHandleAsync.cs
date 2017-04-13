using System.Threading.Tasks;

namespace DDDLite
{
    public interface IHandleAsync<in TMessage>
    {
        Task HandleAsync(TMessage message);
    }
}
