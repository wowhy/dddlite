namespace DDDLite
{
    public interface IConcurrencyVersion
    {
        long RowVersion { get; set; }
    }
}
