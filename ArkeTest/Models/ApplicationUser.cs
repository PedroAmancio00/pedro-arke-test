using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace ArkeTest.Models
{
    [Index(nameof(RefreshToken))]
    public class ApplicationUser : IdentityUser
    {
        public string? RefreshToken { get; set; }
        public DateTime RefreshTokenExpiryTime { get; set; }
    }
}
