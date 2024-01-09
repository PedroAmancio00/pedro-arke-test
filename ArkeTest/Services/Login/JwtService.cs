using ArkeTest.DTO;
using ArkeTest.Models;
using ArkeTest.Services.Login.ILogin;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;
using System.Text;

namespace ArkeTest.Services.Login
{
    public class JwtService(UserManager<ApplicationUser> userManager,
                            IHttpContextAccessor httpContextAccessor,
                            ILogger<JwtService> logger) : IJwtService
    {
        private readonly UserManager<ApplicationUser> _userManager = userManager;
        private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;
        private readonly ILogger<JwtService> _logger = logger;

        public void GenerateJwtToken(ApplicationUser user)
        {
            try
            {
                SymmetricSecurityKey securityKey = new(Encoding.UTF8.GetBytes("bf5ec0cf8bdd34c7508f8d40a7df96b32ff4f2699b96f88076dc9b746b01eb82"));
                SigningCredentials credentials = new(securityKey, SecurityAlgorithms.HmacSha256);

#pragma warning disable CS8604 // Possible null reference argument.
                Claim[] claims =
                [
                    new(JwtRegisteredClaimNames.Sub, user.UserName),
                    new(JwtRegisteredClaimNames.Jti, user.Id),
                ];
#pragma warning restore CS8604 // Possible null reference argument.

                JwtSecurityToken token = new(
                    claims: claims,
                    expires: DateTime.Now.AddMinutes(60),
                    signingCredentials: credentials);

                string jwt = new JwtSecurityTokenHandler().WriteToken(token);

                CookieOptions jwtCookieOptions = new()
                {
                    HttpOnly = true,
                    Secure = true,
                    Expires = DateTime.Now.AddHours(1)
                };

                _httpContextAccessor.HttpContext?.Response.Cookies.Append("jwt", jwt, jwtCookieOptions);

                _logger.LogInformation("JWT generated successfully");

                return;

            }
            catch (Exception)
            {
                _logger.LogInformation("Error generating JWT");
                throw;
            }
        }


        public async Task GenerateRefreshToken(ApplicationUser user)
        {
            try
            {
                string refreshToken = Guid.NewGuid().ToString().Replace("-", "");
                user.RefreshToken = refreshToken;
                user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(7).ToUniversalTime();

                IdentityResult? result = await _userManager.UpdateAsync(user);

                if (result != null && result.Succeeded)
                {

                    CookieOptions refreshTokenCookieOptions = new()
                    {
                        HttpOnly = true,
                        Secure = true,
                        Expires = DateTime.UtcNow.AddDays(7)
                    };

                    _httpContextAccessor.HttpContext?.Response.Cookies.Append("refreshToken", refreshToken, refreshTokenCookieOptions);

                    _logger.LogInformation("Refresh Token generated successfully");
                }

                return;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error generating Refresh Token");
                throw;
            }
        }

        public void RemoveTokens()
        {
            try
            {
                _httpContextAccessor.HttpContext?.Response.Cookies.Delete("jwt");
                _httpContextAccessor.HttpContext?.Response.Cookies.Delete("refreshToken");
                _logger.LogInformation("Tokens removed");
                return;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error removing tokens");
                throw;
            }
        }

        public string? GetRefreshToken()
        {
            try
            {
                string? token = null;

                _httpContextAccessor.HttpContext?.Request.Cookies.TryGetValue("refreshToken", out token);

                return token;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting refresh");
                throw;
            }
        }
    }
}
