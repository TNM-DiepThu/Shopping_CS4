using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Shopping_Application.Models;
namespace Shopping_Application.Configurations
{
    public class ProductConfiguration : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.HasKey(p => p.Id);
            builder.Property(p => p.Name).HasColumnType("nvarchar(100)");
            builder.Property(p => p.Description).IsUnicode(true).
                HasMaxLength(100).IsFixedLength();
            // 2 cái trên tương đương nhau nhé =))
            builder.Property(p => p.Supplier).HasColumnType("nvarchar(100)");

        }
    }
}
