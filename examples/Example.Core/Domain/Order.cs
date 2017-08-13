namespace Example.Core.Domain
{
    using System;
    using System.Collections.Generic;
    using DDDLite.Domain;

    public enum OrderStatus
    {
        New
    }

    public class Order : AggregateRoot
    {
        public string Contact { get; set; }

        public string Mobile { get; set; }

        public string Address { get; set; }

        public DateTime OrderTime { get; set; }

        public decimal TotalPrice { get; set; }

        public OrderStatus Status { get; set; }

        public List<OrderLine> OrderLines { get; set; }
    }
}