using System;
using System.ComponentModel.DataAnnotations;

namespace ProductLab.Models
{
    public class Product
    {
        public Guid ProductId { get; set; }

        [Required(ErrorMessage = "Product name is required")]
        public string Name { get; set; } = string.Empty;

        public string? Description { get; set; }

        [Required]
        [Range(0.01, double.MaxValue, ErrorMessage = "Price must be greater than 0")]
        public decimal Price { get; set; }

        [Required]
        [Range(0, int.MaxValue, ErrorMessage = "Stock quantity cannot be negative")]
        public int StockQuantity { get; set; }

        public DateTime CreatedAt { get; set; }
    }
}
