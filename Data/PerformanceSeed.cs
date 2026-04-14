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
                new Author { Id = Guid.NewGuid(), UserName = "Noah" }
            };

            db.Authors.AddRange(authors);
            await db.SaveChangesAsync();

            // -----------------------
            // 2. PRODUCTS
            // -----------------------
            var productNames = new List<string>
            {
                "Gaming Laptop Pro X1",
                "Wireless Noise Cancelling Headphones",
                "Smart Fitness Watch Series 5",
                "4K Ultra HD Monitor 27 inch",
                "Mechanical Gaming Keyboard RGB",
                "Ergonomic Office Chair Pro",
                "Portable SSD 1TB Extreme",
                "Smartphone Pro Max Ultra",
                "Bluetooth Speaker Boom 360",
                "Action Camera 5K Pro"
            };

            var products = new List<Product>();

            foreach (var name in productNames)
            {
                products.Add(new Product
                {
                    Id = Guid.NewGuid(),
                    Name = name,
                    Description = "Sample product description",
                    Price = 99.90m

                });
            }

            db.Products.AddRange(products);
            await db.SaveChangesAsync();

            // -----------------------
            // 3. REVIEWS (IMPORTANT PART)
            // -----------------------
            var random = new Random();
            var reviews = new List<Review>();

            foreach (var product in products)
            {
                for (int i = 0; i < 5; i++)
                {
                    var author = authors[random.Next(authors.Count)];

                    reviews.Add(new Review
                    {
                        Id = Guid.NewGuid(),
                        Content = $"Review {i + 1} for {product.Name}",
                        Rating = random.Next(1, 11),
                        ProductId = product.Id,
                        AuthorId = author.Id
                    });
                }
            }

            db.Reviews.AddRange(reviews);
            await db.SaveChangesAsync();
        }
    }
}