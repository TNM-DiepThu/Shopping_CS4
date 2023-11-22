using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Shopping_Application.Models;
namespace Shopping_Application.Configurations
{
    public class BillDetailConfiguration : IEntityTypeConfiguration<BillDetail>
    {
        public void Configure(EntityTypeBuilder<BillDetail> builder)
        {
            builder.HasKey(p => p.ID);
            // Set thuộc tính
            builder.Property(p => p.Price).HasColumnType("int");
            builder.Property(c => c.Quantity).HasColumnType("int");
            // Set khóa ngoại
            builder.HasOne(x => x.Bill).WithMany(y => y.BillDetails).
                HasForeignKey(c => c.IDHD);
            builder.HasOne(x => x.Product).WithMany(y => y.BillDetails).
                HasForeignKey(c => c.IDSP);
        }
    }
}
