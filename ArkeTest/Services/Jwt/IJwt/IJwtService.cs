using ArkeTest.Models;
using Microsoft.IdentityModel.Tokens;

namespace ArkeTest.Services.Jwt.IJwt
{
    public interface IJwtService
    {
        void GenerateJwtToken(ApplicationUser user);
        Task GenerateRefreshToken(ApplicationUser user);
        void RemoveTokens();
        string? GetRefreshToken();
        string? GetAndDecodeJwtToken();
    }

}
