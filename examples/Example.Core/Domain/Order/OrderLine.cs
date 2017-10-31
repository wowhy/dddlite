namespace Example.Core.Domain
{
  using System;
  using DDDLite.Domain;

  public class OrderLine : Entity<Guid>
  {
    public Guid OrderId { get; set; }

    public Guid ProductId { get; set; }

    public string ProductName { get; set; }

    public int Count { get; set; }

    public decimal UnitPrice { get; set; }

    public decimal TotalPrice { get; set; }
  }
}