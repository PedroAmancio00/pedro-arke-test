using ArkeTest.DTO;
using ArkeTest.Models;
using ArkeTest.Services.Jwt.IJwt;
using ArkeTest.Services.Login.ILogin;
using Microsoft.AspNetCore.Identity;
using System.Net;

namespace ArkeTest.Services.Login
{
    public class AccessAccountService : IAccessAccountService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IJwtService _jwtService;
        private readonly ILogger<AccessAccountService> _logger;

        public AccessAccountService(UserManager<ApplicationUser> userManager, IJwtService jwtService, ILogger<AccessAccountService> logger)
        {
            _userManager = userManager;
            _jwtService = jwtService;
            _logger = logger;
        }

        public async Task<ReturnDTO> AccessAccount(AccessAccountDTO dto)
        {
            try
            {
                // Find the user with the email
                ApplicationUser? login = await _userManager.FindByEmailAsync(dto.Email);

                // If the user is null, return a 404
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

                // Check if the password is correct
                bool isPasswordCorrect = await _userManager.CheckPasswordAsync(login, dto.Password);

                // If the password is not correct, return a 409
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

                // If the password is correct, generate a new JWT token
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

            // If there is an error, return a 500
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
