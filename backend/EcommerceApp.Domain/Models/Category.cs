﻿using System.ComponentModel.DataAnnotations;

namespace EcommerceApp.Domain.Models
{
    public class Category
    {

        public int Id { get; set; }
        public string Name { get; set; } = default!;
        public string Slug { get; set; } = default!;
        public ICollection<Product> Products { get; set; } = new List<Product>();
    }
}