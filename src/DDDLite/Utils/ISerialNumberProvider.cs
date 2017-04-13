namespace DDDLite.Utils
{
    public interface ISerialNumberProvider : IProvider
    {
        string GetSerialNumber(string type);
        string GetSerialNumber(string type, string dynamicContent);
    }
}