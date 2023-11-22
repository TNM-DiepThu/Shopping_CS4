using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Shopping_Application.Models;

namespace Shopping_Application.Configurations
{
    public class BillConfigurationL : IEntityTypeConfiguration<Bill>
    {
        public void Configure(EntityTypeBuilder<Bill> builder)
        {
            builder.ToTable("HoaDon"); // Đặt tên cho bảng
            builder.HasKey(x => x.Id); // Khóa chính
            // Set các thuộc tính
            builder.Property(x => x.CreateDate).HasColumnType("Datetime").
                IsRequired(); // Datetime  not null
            builder.Property(x => x.Status).HasColumnType("nvarchar(1000)").
                IsRequired(); // nvarchar(1000) not null

        }
    }
}
