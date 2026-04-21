using Microsoft.EntityFrameworkCore;
using PerformanceTest.Models;

namespace PerformanceTest.Data
{
    public class SeedData
    {
        public static async Task InitializeAsync(PerformanceDbContext db)
        {
            // Prevent duplicate seeding
            if (await db.Products.AnyAsync())
                return;

            // -----------------------
            // 1. AUTHORS
            // -----------------------
            var authors = new List<Author>
            {
                new Author { Id = Guid.NewGuid(), UserName = "Alex" },
                new Author { Id = Guid.NewGuid(), UserName = "Emma" },
                new Author { Id = Guid.NewGuid(), UserName = "Noah" },
                new Author { Id = Guid.NewGuid(), UserName = "Jane" },
                new Author { Id = Guid.NewGuid(), UserName = "John" },
                new Author { Id = Guid.NewGuid(), UserName = "Doe" }
            };

            db.Authors.AddRange(authors);
            await db.SaveChangesAsync();

            // -----------------------
            // 2. PRODUCTS
            // -----------------------


            var random = new Random();
            var batchSize = 1000;
            var totalProducts = 50000;

            for (int i = 1; i <= totalProducts; i++)
            {
                var productId = Guid.NewGuid();
                var product = new Product
                {
                    Id = productId,
                    Name = $"Product_Batch_{i}",
                    Description = "Bulk seeded product for indexing test.",
                    Price = (decimal)(random.NextDouble() * 1000)
                };
                db.Products.Add(product);

                // Add 5 reviews for every product to create "deep" data
                for (int j = 1; j <= 5; j++)
                {
                    db.Reviews.Add(new Review
                    {
                        Id = Guid.NewGuid(),
                        Content = $"Review {j} for product {i}",
                        Rating = random.Next(1, 11),
                        ProductId = productId,
                        AuthorId = authors[random.Next(authors.Count)].Id
                    });
                }

                // Save in batches to avoid memory overflow in Docker
                if (i % batchSize == 0)
                {
                    await db.SaveChangesAsync();
                    Console.WriteLine($"Seeded {i} products and {i * 5} reviews...");
                }
            }



        }
    }
}