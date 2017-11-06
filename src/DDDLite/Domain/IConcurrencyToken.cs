namespace DDDLite.Domain
{
  public interface IConcurrencyToken
  {
    long Version { get; set; }
  }
}
