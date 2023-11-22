using Microsoft.EntityFrameworkCore;
using Shopping_Application.Configurations;
using System.Reflection;

namespace Shopping_Application.Models
{
    public class ShopDbContext : DbContext
    {
        public ShopDbContext() {}
        public ShopDbContext(DbContextOptions options):base(options) { }
        // DbSet
        public DbSet<Product> Products { get; set;}
        public DbSet<Bill> Bills { get; set; }
        public DbSet<BillDetail> BillDetails { get; set; }
        public DbSet<Cart> Carts { get; set; }
        public DbSet<CartDetail> CartDetails { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<User> Users { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"Data Source=DESKTOP-RNC4UFP\SQLEXPRESS;Initial Catalog=IT17303_ShopHoclai;Integrated Security=True");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.
                ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }
    }
}
