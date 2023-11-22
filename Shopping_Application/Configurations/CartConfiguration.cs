using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Shopping_Application.Models;
namespace Shopping_Application.Configurations
{
    public class CartConfiguration : IEntityTypeConfiguration<Cart>
    {
        public void Configure(EntityTypeBuilder<Cart> builder)
        {
            // If you don't use ToTable function, the table's name will
            // be the name of the class model 
            // Nếu ta không dùng phương thức ToTable thì tên của bảng sẽ chính
            // là tên của class model
            builder.HasKey(p => p.UserId);
            builder.Property(p => p.Description).HasColumnType("nvarchar(1000)");

        }
    }
}
