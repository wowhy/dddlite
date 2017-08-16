namespace Example.Core.Domain
{
    using System;
    using DDDLite.Domain;
    public class Product : AggregateRoot, ILogicalDelete
    {
        public string Code { get; set; }

        public string Name { get; set; }

        public ProductDetail Detail { get; set; }

        public bool Deleted { get; set; }
    }
}