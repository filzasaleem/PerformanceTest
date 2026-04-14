using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PerformanceTest.Models
{
    public class Author
    {
        public Guid Id { get; set; }
        public string UserName { get; set; }
        public virtual List<Review> Reviews { get; set; } = new();
    }
}