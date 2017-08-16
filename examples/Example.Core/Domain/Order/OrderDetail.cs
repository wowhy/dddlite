namespace Example.Core.Domain
{
    using DDDLite.Domain;

    public class OrderDetail
    {
        public StreetAddress BillingAddress { get; set; }
        public StreetAddress ShippingAddress { get; set; }
    }
}