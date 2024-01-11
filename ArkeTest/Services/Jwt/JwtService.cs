using ArkeTest.Models;
using ArkeTest.Services.Factory.IFactory;
using ArkeTest.Services.Jwt.IJwt;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ArkeTest.Services.Jwt
{
    public class JwtService : IJwtService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ILogger<JwtService> _logger;
        private readonly IConfiguration _configuration;
        private readonly ITokenValidationParametersFactory _factoryTokenValidationParametersFactory;
        private readonly ITokenHandlerWrapper _tokenHandlerWrapper;

        public JwtService(UserManager<ApplicationUser> userManager,
                          IHttpContextAccessor httpContextAccessor,
                          ILogger<JwtService> logger,
                          IConfiguration configuration,
                          ITokenValidationParametersFactory factoryTokenValidationParametersFactory,
                          ITokenHandlerWrapper tokenHandlerWrapper)
        {
            _userManager = userManager;
            _httpContextAccessor = httpContextAccessor;
            _logger = logger;
            _configuration = configuration;
            _factoryTokenValidationParametersFactory = factoryTokenValidationParametersFactory;
            _tokenHandlerWrapper = tokenHandlerWrapper;
        }

        public void GenerateJwtToken(ApplicationUser user)
        {
            try
            {
                string secret = _configuration["jwtKey"]!;
                // Create the security key
                SymmetricSecurityKey securityKey = new(Encoding.UTF8.GetBytes(secret));
                SigningCredentials credentials = new(securityKey, SecurityAlgorithms.HmacSha256);

                // Create the claims
#pragma warning disable CS8604 // Possible null reference argument.
                Claim[] claims =
                [
                    new(JwtRegisteredClaimNames.Sub, user.UserName),
                    new(JwtRegisteredClaimNames.Jti, user.Id),
                ];
#pragma warning restore CS8604 // Possible null reference argument.

                // Create the token
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

                // Add the token to the cookie
                _httpContextAccessor.HttpContext?.Response.Cookies.Append("jwt", jwt, jwtCookieOptions);

                _logger.LogInformation("JWT generated successfully");

                return;

            }
            // If there is an error, throw an exception
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
                // Generate a refresh token
                string refreshToken = Guid.NewGuid().ToString().Replace("-", "");
                user.RefreshToken = refreshToken;
                user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(7).ToUniversalTime();

                // Update the user
                IdentityResult? result = await _userManager.UpdateAsync(user);

                // If the user is updated, add the refresh token to the cookie
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
            // If there is an error, throw an exception
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
                // Remove the tokens from the cookie
                _httpContextAccessor.HttpContext?.Response.Cookies.Delete("jwt");
                _httpContextAccessor.HttpContext?.Response.Cookies.Delete("refreshToken");
                _logger.LogInformation("Tokens removed");
                return;
            }
            // If there is an error, throw an exception
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
                // Get the refresh token from the cookie
                string? token = null;

                _httpContextAccessor.HttpContext?.Request.Cookies.TryGetValue("refreshToken", out token);

                return token;
            }
            // If there is an error, throw an exception
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting refresh");
                throw;
            }
        }

        public string? GetAndDecodeJwtToken()
        {
            try
            {
                // Get the refresh token from the cookie
                string? token = null;

                _httpContextAccessor.HttpContext?.Request.Cookies.TryGetValue("jwt", out token);

                string secret = _configuration["jwtKey"]!;

                TokenValidationParameters validationParameters = _factoryTokenValidationParametersFactory.Create(secret);

                ClaimsPrincipal principal = _tokenHandlerWrapper.ValidateToken(token, validationParameters, out SecurityToken validatedToken);

                IEnumerable<Claim> claims = principal.Claims;

                foreach (Claim claim in claims)
                {
                    if (claim.Type == "jti")
                    {
                        return claim.Value;
                    }
                }

                return null;
            }
            // If there is an error, throw an exception
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting jwt");
                throw;
            }
        }
    }
}
