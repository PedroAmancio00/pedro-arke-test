using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace ArkeTest.Models
{
    [Index(nameof(Name))]
    [Index(nameof(Description))]
    public class Product : BaseEntity
    {
        [Required]
        [MaxLength(100)]
        public string Name { get; set; }

        [Required]
        [MaxLength(500)]
        public string Description { get; set; }

        [Required]
        [Precision(10, 2)]        
        public decimal Price { get; set; }

        [Required]
        public int Quantity { get; set; }

        [Required]
        public Guid UserInformationId { get; set; }

        [ForeignKey("UserInformationId")]
        public virtual UserInformation? UserInformation { get; set; }

        public Product(string name, string description, decimal price, int quantity, Guid userInformationId ) : base()
        {
            Name = name;
            Description = description;
            Price = price;
            Quantity = quantity;
            UserInformationId = userInformationId;
        }
        
    }
}
