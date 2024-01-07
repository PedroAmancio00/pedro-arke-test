using ArkeTest.DTO;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;
using System.Text;

namespace ArkeTest.Services.Login
{
    public class AccessAccountService
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly ILogger<AccessAccountService> _logger;

        public AccessAccountService(UserManager<IdentityUser> userManager, ILogger<AccessAccountService> logger)
        {
            _userManager = userManager;
            _logger = logger;
        }

        public async Task<ReturnDTO> AccessAccount(AccessAccountDTO dto)
        {
            try
            {
                IdentityUser? login = await _userManager.FindByEmailAsync(dto.Email);

                if (login == null)
                {
                    ReturnDTO returnDTO = new()
                    {
                        Message = "Email or Password is wrong",
                        StatusCode = HttpStatusCode.Conflict
                    };
                    _logger.LogInformation("Login not found");

                    return returnDTO;
                }

                bool isPasswordCorrect = await _userManager.CheckPasswordAsync(login, dto.Password);

                if (!isPasswordCorrect)
                {
                    ReturnDTO returnDTO = new()
                    {
                        Message = "Email or Password is wrong",
                        StatusCode = HttpStatusCode.Conflict
                    };
                    _logger.LogInformation("Wrong password");

                    return returnDTO;
                }

                else
                {
                    ReturnDTO returnDTO = new()
                    {
                        Message = "Successful Login",
                        StatusCode = HttpStatusCode.OK
                    };
                    _logger.LogInformation("successful Login");

                    return returnDTO;
                }

            }
            catch (Exception e)
            {
                _logger.LogError(e, "Internal error on login");

                ReturnDTO returnDTO = new()
                {
                    Message = "Error on login",
                    StatusCode = HttpStatusCode.InternalServerError
                };

                return returnDTO;
            }

        }


        public string GenerateJwtToken(string id, string username)
        {
            SymmetricSecurityKey securityKey = new(Encoding.UTF8.GetBytes("Hvg8-.Ua7pvtJLFxvJCR"));
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

        /*
        public async Task<bool> SaveRefreshToken(IdentityUser user, string refreshToken)
        {
            user.RefreshToken = refreshToken;
            user.RefreshTokenExpiryTime = DateTime.Now.AddDays(7);

            var result = await _userManager.UpdateAsync(user);
            return result.Succeeded;
        }
        */
    }
}
