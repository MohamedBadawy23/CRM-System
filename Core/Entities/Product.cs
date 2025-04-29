using System;
using System.Collections.Generic;

namespace Core.Entities
{
    public class Product : BaseEntity
    {
        public string? Name { get; set; }
        public string? Description { get; set; }
        public decimal? Price { get; set; }
        public int? Stock { get; set; }
        public string? Category { get; set; }
        public DateTime? CreatedDate { get; set; } = DateTime.Now;
        public string? UserId { get; set; }
        public User? User { get; set; }
    }
} 