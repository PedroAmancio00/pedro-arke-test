using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ArkeTest.Models
{
    [Index(nameof(Name))]
    [Index(nameof(Description))]
    public class Product : BaseEntity
    {
        [MaxLength(100)]
        public string Name { get; set; }
        [MaxLength(500)]
        public string Description { get; set; }
        [Precision(10, 2)]
        public decimal Price { get; set; }

        public int Quantity { get; set; }

        public bool IsActive { get; set; }

        public Guid UserId { get; set; }

        [ForeignKey("UserId")]
        public virtual User? User { get; set; }

        public Product(string name, string description, decimal price, int quantity, Guid userId)
        {
            Name = name;
            Description = description;
            Price = price;
            Quantity = quantity;
            IsActive = true;
            UserId = userId;
        }
    }
}
