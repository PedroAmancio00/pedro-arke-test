using System.ComponentModel.DataAnnotations;

namespace ArkeTest.DTO.User
{
    public class CreateUserDTO
    {
        [MaxLength(100)]
        [Required]
        public string Name { get; set; } = null!;

        [MaxLength(100)]
        public string? AddressLine1 { get; set; }

        [MaxLength(100)]
        public string? AddressLine2 { get; set; }
    }
}
