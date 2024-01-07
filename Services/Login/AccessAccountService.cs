using ArkeTest.DTO;
using ArkeTest.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;
using System.Text;

namespace ArkeTest.Services.Login
{
    public class AccessAccountService(UserManager<ApplicationUser> userManager, ILogger<AccessAccountService> logger)
    {
        private readonly UserManager<ApplicationUser> _userManager = userManager;
        private readonly ILogger<AccessAccountService> _logger = logger;

        public async Task<ReturnJwtDTO> AccessAccount(AccessAccountDTO dto)
        {
            try
            {
                ApplicationUser? login = await _userManager.FindByEmailAsync(dto.Email);

                if (login == null)
                {
                    ReturnJwtDTO returnDTO = new()
                    {
                        Message = "Email or Password is wrong",
                        StatusCode = HttpStatusCode.Conflict,
                    };
                    _logger.LogInformation("Login not found");

                    return returnDTO;
                }

                bool isPasswordCorrect = await _userManager.CheckPasswordAsync(login, dto.Password);

                if (!isPasswordCorrect)
                {
                    ReturnJwtDTO returnDTO = new()
                    {
                        Message = "Email or Password is wrong",
                        StatusCode = HttpStatusCode.Conflict
                    };
                    _logger.LogInformation("Wrong password");

                    return returnDTO;
                }

                else
                {
                    var tokens = await GenerateJwtAndRefreshToken(login);

                    ReturnJwtDTO returnDTO = new()
                    {
                        Message = "Successful Login",
                        StatusCode = HttpStatusCode.OK,
                        JwtToken = tokens.Item1,
                        RefreshToken = tokens.Item2
                    };
                    _logger.LogInformation("successful Login");

                    return returnDTO;
                }

            }
            catch (Exception e)
            {
                _logger.LogError(e, "Internal error on login");

                ReturnJwtDTO returnDTO = new()
                {
                    Message = "Error on login",
                    StatusCode = HttpStatusCode.InternalServerError
                };

                return returnDTO;
            }

        }

        private async Task<(string, string)> GenerateJwtAndRefreshToken(ApplicationUser user)
        {

#pragma warning disable CS8604 // Possible null reference argument.
            var jwtToken = GenerateJwtToken(user.Id, user.UserName);
#pragma warning restore CS8604 // Possible null reference argument.

            var refreshToken = Guid.NewGuid().ToString().Replace("-", "");

            await SaveRefreshToken(user, refreshToken);

            return (jwtToken, refreshToken);
        }


        public string GenerateJwtToken(string id, string username)
        {
            SymmetricSecurityKey securityKey = new(Encoding.UTF8.GetBytes("bf5ec0cf8bdd34c7508f8d40a7df96b32ff4f2699b96f88076dc9b746b01eb82"));
            SigningCredentials credentials = new(securityKey, SecurityAlgorithms.HmacSha256);

            Claim[] claims =
            {
                new(JwtRegisteredClaimNames.Sub, username),
                new(JwtRegisteredClaimNames.Jti, id),
            };

            JwtSecurityToken token = new(
                issuer: "YourIssuer",
                audience: "YourAudience",
                claims: claims,
                expires: DateTime.Now.AddMinutes(60),
                signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }


        private async Task<bool> SaveRefreshToken(ApplicationUser user, string refreshToken)
        {
            user.RefreshToken = refreshToken;
            user.RefreshTokenExpiryTime = DateTime.Now.AddDays(7).ToUniversalTime();

            var result = await _userManager.UpdateAsync(user);
            return result.Succeeded;
        }

    }
}
