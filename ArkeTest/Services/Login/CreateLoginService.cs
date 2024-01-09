using ArkeTest.DTO;
using ArkeTest.DTO.Login;
using ArkeTest.Models;
using ArkeTest.Services.Login.ILogin;
using Microsoft.AspNetCore.Identity;
using System.Net;

namespace ArkeTest.Services.Login
{
    public class CreateLoginService(UserManager<ApplicationUser> userManager, ILogger<CreateLoginService> logger) : ICreateLoginService
    {
        private readonly UserManager<ApplicationUser> _userManager = userManager;
        private readonly ILogger<CreateLoginService> _logger = logger;

        public async Task<ReturnDTO> CreateLogin(CreateLoginDTO dto)
        {
            try
            {
                ApplicationUser login = new()
                {
                    Email = dto.Email,
                    UserName = dto.Email
                };

                // Creating login
                IdentityResult? result = await _userManager.CreateAsync(login, dto.Password);

                // If the login is not created, return a 409
                if (!result.Succeeded)
                {
                    string message = result.Errors.Select(e => e.Description).Aggregate((a, b) => $"{a}, {b}");

                    ReturnDTO returnDTO = new()
                    {
                        Message = message,
                        StatusCode = HttpStatusCode.Conflict
                    };
                    _logger.LogInformation("Error creating login");
                    return returnDTO;
                }

                else
                {
                    // If the login is created, return a 201
                    ReturnDTO returnDTO = new()
                    {
                        Message = "Login Created",
                        StatusCode = HttpStatusCode.Created
                    };
                    _logger.LogInformation("Login created");

                    return returnDTO;
                }
            }
            // If there is an error, return a 500
            catch (Exception ex)
            {                
                _logger.LogError(ex, "Internal error creating login");

                ReturnDTO returnDTO = new()
                {
                    Message = "Error creating login",
                    StatusCode = HttpStatusCode.InternalServerError
                };

                return returnDTO;
            }
        }
    }
}
