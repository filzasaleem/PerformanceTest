using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PerformanceTest.Models;

namespace PerformanceTest.Data
{
    public class PerformanceDbContext : DbContext
    {
        public PerformanceDbContext(DbContextOptions<PerformanceDbContext> options) : base(options)
        { }
        public DbSet<Product> Products { get; set; }
        public DbSet<Author> Authors { get; set; }
        public DbSet<Review> Reviews { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            //product has many reviews
            modelBuilder.Entity<Product>().HasMany(p => p.Reviews).WithOne(r => r.Product).HasForeignKey(r => r.ProductId).OnDelete(DeleteBehavior.Cascade);
            //Author has many reviews
            // modelBuilder.Entity<Author>().HasMany(a => a.Reviews).WithOne(r => r.Author).HasForeignKey(r => r.AuthorId).OnDelete(DeleteBehavior.Cascade);
        }

    }
}