using ArkeTest.DTO;
using ArkeTest.Models;
using ArkeTest.Services.Login.ILogin;
using Microsoft.AspNetCore.Identity;
using System.Net;

namespace ArkeTest.Services.Login
{
    public class AccessAccountService(UserManager<ApplicationUser> userManager, IJwtService jwtService, ILogger<AccessAccountService> logger) : IAccessAccountService
    {
        private readonly UserManager<ApplicationUser> _userManager = userManager;
        private readonly IJwtService _jwtService = jwtService;
        private readonly ILogger<AccessAccountService> _logger = logger;

        public async Task<ReturnDTO> AccessAccount(AccessAccountDTO dto)
        {
            try
            {
                ApplicationUser? login = await _userManager.FindByEmailAsync(dto.Email);

                if (login == null)
                {
                    ReturnDTO returnDTO = new()
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
                    _jwtService.GenerateJwtToken(login);

                    await _jwtService.GenerateRefreshToken(login);

                    ReturnDTO returnDTO = new()
                    {
                        Message = "Successful Login",
                        StatusCode = HttpStatusCode.OK
                    };
                    _logger.LogInformation("successful Login");

                    return returnDTO;
                }

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Internal error on login");

                ReturnDTO returnDTO = new()
                {
                    Message = "Error on login",
                    StatusCode = HttpStatusCode.InternalServerError
                };

                return returnDTO;
            }

        }
    }
}
