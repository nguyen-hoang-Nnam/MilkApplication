using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using MilkApplication.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MilkApplication.DAL.Data
{
    public class AppDbContext : IdentityDbContext<ApplicationUser>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
        public DbSet<Product> Products { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Origin> Origins { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Vouchers> Vouchers { get; set; }
        public DbSet<Location> Locations { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderDetail> OrderItems { get; set; }
        public DbSet<Combo> Combos { get; set; }
        public DbSet<ComboProduct> ComboProducts { get; set; }
        public DbSet<Payment> Payments { get; set; }
        public DbSet<PaymentMethod> PaymentMethods { get; set; }
        public DbSet<PaymentCallBack> PaymentCallBacks { get; set; }
        public DbSet<Address> Addresses { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            //Configure Voucher - Order relationship
            modelBuilder.Entity<Order>()
                .HasOne(o => o.Voucher)
                .WithMany(v => v.Orders)
                .HasForeignKey(o => o.voucherId)
                .OnDelete(DeleteBehavior.Cascade);
            // Configure Category - Product relationship
            modelBuilder.Entity<Product>()
                .HasOne(p => p.Category)
                .WithMany(c => c.Products)
                .HasForeignKey(p => p.categoryId)
                .OnDelete(DeleteBehavior.Cascade);

            // Configure Origin - Product relationship
            modelBuilder.Entity<Product>()
                .HasOne(p => p.Origin)
                .WithMany(o => o.Products)
                .HasForeignKey(p => p.originId)
                .OnDelete(DeleteBehavior.Cascade);
            // Configure Comment - Product relationship
            modelBuilder.Entity<Comment>()
                .HasOne(c => c.Product)
                .WithMany(p => p.Comments)
                .HasForeignKey(c => c.productId)
                .OnDelete(DeleteBehavior.Cascade);
            // Configure Comment - User relationship
            modelBuilder.Entity<Comment>()
                .HasOne(c => c.User)
                .WithMany(u => u.Comments)
                .HasForeignKey(c => c.Id)
                .OnDelete(DeleteBehavior.Cascade);
            // Configure Product - Location relationship
            modelBuilder.Entity<Product>()
                .HasOne(l => l.Location)
                .WithMany(p => p.Products)
                .HasForeignKey(l => l.locationId)
                .OnDelete(DeleteBehavior.Cascade);
            // Configure User - Address relationship
            modelBuilder.Entity<Address>()
                .HasOne(c => c.User)
                .WithMany(u => u.Addresses)
                .HasForeignKey(c => c.Id);

            modelBuilder.Entity<Order>(entity =>
            {
                entity.Property(e => e.totalPrice)
                    .HasColumnType("decimal(18, 2)");
            });

            modelBuilder.Entity<OrderDetail>(entity =>
            {
                entity.Property(e => e.Price)
                    .HasColumnType("decimal(18, 2)");
            });

            modelBuilder.Entity<Product>(entity =>
            {
                entity.Property(e => e.Price)
                    .HasColumnType("decimal(18, 2)");
            });

            modelBuilder.Entity<Order>()
                .HasMany(o => o.OrderDetails)
                .WithOne(oi => oi.Order)
                .HasForeignKey(oi => oi.orderId)
                .OnDelete(DeleteBehavior.Cascade);

            // Configure the many-to-many relationship
            /*modelBuilder.Entity<ComboProduct>()
            .HasKey(cp => new { cp.comboProductId});*/
            
            modelBuilder.Entity<ComboProduct>()
                .HasOne(cp => cp.Combo)
                .WithMany(c => c.ComboProducts)
                .HasForeignKey(cp => cp.comboId);

            modelBuilder.Entity<ComboProduct>()
                .HasOne(cp => cp.Product)
                .WithMany(p => p.ComboProducts)
                .HasForeignKey(cp => cp.productId);

            modelBuilder.Entity<Product>()
                .HasOne(p => p.User)
                .WithMany(u => u.Products)
                .HasForeignKey(p => p.Id);
        }
    }
}
