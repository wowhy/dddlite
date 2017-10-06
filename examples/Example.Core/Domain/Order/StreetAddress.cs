namespace Example.Core.Domain
{
  using DDDLite.Domain;

  public class StreetAddress : ValueObject<StreetAddress>
  {
    public string Street { get; set; }
    public string City { get; set; }
  }
}