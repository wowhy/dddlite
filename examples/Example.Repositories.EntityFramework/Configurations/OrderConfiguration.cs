namespace Example.Repositories.EntityFramework.Configurations
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    using Example.Core.Domain;

    public class OrderConfiguration : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {
            builder.HasMany(k => k.OrderLines)
                .WithOne()
                .HasForeignKey(k => k.OrderId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.OwnsOne(p => p.Detail, cb =>
            {
                cb.OwnsOne(c => c.BillingAddress);
                cb.OwnsOne(c => c.ShippingAddress);
            });
        }
    }
}