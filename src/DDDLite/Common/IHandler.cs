namespace DDDLite.Common
{
    using System.Threading.Tasks;
    
    public interface IHandler<in TMessage>
    {
        void Handle(TMessage message);
    }
}