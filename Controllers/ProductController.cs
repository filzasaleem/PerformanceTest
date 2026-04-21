using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PerformanceTest.Data;

namespace PerformanceTest.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductController : ControllerBase
    {
        private readonly PerformanceDbContext _db;
        public ProductController(PerformanceDbContext db)
        {
            _db = db;
        }
        [HttpGet("bad-performance")]
        public List<ProductDto> GetProductsBad()
        {

            var products = _db.Products.ToList();

            // 2. Map them manually to DTOs (This triggers the N+1)
            var result = products.Select(p => new ProductDto
            {
                Name = p.Name,
                ReviewCount = p.Reviews.Count // N+1 Trigger 1
            }).ToList();

            return result;

        }
        [HttpGet("good-performance")]
        public List<ProductDto> GetProductsGood()
        {
            var products = _db.Products.Include(p => p.Reviews).ToList();

            var result = products.Select(p => new ProductDto
            {
                Name = p.Name,
                ReviewCount = p.Reviews.Count

            }).ToList();
            return result;
        }
        [HttpGet("projection-performance")]
        public List<ProductDto> GetProductsProjection()
        {
            var result = _db.Products.Select(p => new ProductDto
            {
                Name = p.Name,
                ReviewCount = p.Reviews.Count()
            }).ToList();
            return result;

        }
        [HttpGet("search/{name}")]
        public async Task<IActionResult> Search(string name)
        {
            var sw = System.Diagnostics.Stopwatch.StartNew();

            var product = await _db.Products
                .FirstOrDefaultAsync(p => p.Name == name);

            sw.Stop();

            if (product == null) return NotFound();

            return Ok(new
            {
                Found = product.Name,
                DurationMs = sw.Elapsed.TotalMilliseconds
            });
        }
    }
}