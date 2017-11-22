namespace DDDLite.Serialization
{
    public interface IObjectSerializer
    {
        byte[] Serialize<T>(T value);

        T Deserialize<T>(byte[] buffer);
    }
}