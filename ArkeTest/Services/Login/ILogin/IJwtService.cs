using ArkeTest.Models;

namespace ArkeTest.Services.Login.ILogin
{
    public interface IJwtService
    {
        void GenerateJwtToken(ApplicationUser user);
        Task GenerateRefreshToken(ApplicationUser user);
        void RemoveTokens();
        string? GetRefreshToken();
    }
}
