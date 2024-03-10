using Microsoft.EntityFrameworkCore;
using WebApplication4.Services.CouponAPI.Models;

namespace WebApplication4.Services.CouponAPI.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<Coupon> Coupons { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Coupon>().HasData(new Coupon
            {
                CouponId = 1,
                CouponCode = "1OOFF",
                DiscountAmount = 10,
                MinAmount = 10
            });

            modelBuilder.Entity<Coupon>().HasData(new Coupon
            {
                CouponId = 2,
                CouponCode = "2OOFF",
                DiscountAmount = 20,
                MinAmount = 40
            });
        }
    }
}
