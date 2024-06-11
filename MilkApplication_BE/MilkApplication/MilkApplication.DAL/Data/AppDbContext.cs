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

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

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
        }
    }
}
