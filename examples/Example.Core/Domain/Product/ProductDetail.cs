namespace Example.Core.Domain
{
    using DDDLite.Domain;

    public class ProductDetail : ValueObject<ProductDetail>
    {
        public string Memo { get; set; }
    }
}