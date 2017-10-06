namespace DDDLite.Domain
{
  public interface IConcurrencyToken
  {
    long RowVersion { get; set; }
  }
}
