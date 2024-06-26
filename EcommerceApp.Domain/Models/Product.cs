﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EcommerceApp.Domain.Models
{
    public class Product
    {
        public Guid Id { get; set; }

        [MaxLength(255)]
        public required string Name { get; set; }

        [MaxLength(1000)]
        public string? Description { get; set; }
        
        [MaxLength(1024)]
        public string? ThumbUrl { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal OriginalPrice { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal SalePrice { get; set; }

        public ICollection<ProductVariant> ProductVariants { get; set; } = new List<ProductVariant>();
        public ICollection<ProductCategory> ProductCategories { get; set; } = new List<ProductCategory>();
    }
}