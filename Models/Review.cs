using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PerformanceTest.Models
{
    public class Review
    {
        public Guid Id { get; set; }
        public string Content { get; set; } = string.Empty;
        public int Rating { get; set; } // between 1-10
        public Guid ProductId { get; set; }
        public virtual Product? Product { get; set; }
        // public Guid AuthorId { get; set; }
        public virtual Author? Author { get; set; }
    }
}