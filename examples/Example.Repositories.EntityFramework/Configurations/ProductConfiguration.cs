namespace Example.Repositories.EntityFramework.Configurations
{

    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    using Example.Core.Domain;
    using DDDLite.Repositories.EntityFramework;

    public class ProductConfiguration : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.OwnsOne(p => p.Detail);
            builder.HasQueryFilterLogicalDelete();
        }
    }
}