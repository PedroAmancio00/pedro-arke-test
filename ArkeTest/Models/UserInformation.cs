using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

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
        public virtual ApplicationUser? Login { get; set; }

        public UserInformation(string name, string loginId, string? addressLine1, string? addressLine2) : base()
        {
            Name = name;
            LoginId = loginId;
            AddressLine1 = addressLine1;
            AddressLine2 = addressLine2;
        }
    }
}
