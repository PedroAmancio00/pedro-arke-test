using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace ArkeTest.Models
{
    public class UserInformation : BaseEntity
    {
        [MaxLength(100)]
        [Required]
        public string Name { get; set; }

        [MaxLength(100)]
        public string? AddressLine1 { get; set; }

        [MaxLength(100)]
        public string? AddressLine2 { get; set; }

        public string? LoginId { get; set; }

        [ForeignKey("LoginId")]
        public virtual IdentityUser? Login { get; set; }

        public UserInformation(string name, string loginId)
        {
            Name = name;
            LoginId = loginId;
        }
    }
}
