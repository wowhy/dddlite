namespace Example.Core.Domain
{
  using DDDLite.Domain;

  public class OrderDetail : ValueObject<OrderDetail>
  {
    public OrderDetail()
    {
      this.BillingAddress = new StreetAddress();
      this.ShippingAddress = new StreetAddress();
    }

    public StreetAddress BillingAddress { get; set; }
    public StreetAddress ShippingAddress { get; set; }
  }
}