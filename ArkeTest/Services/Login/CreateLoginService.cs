using ArkeTest.DTO;
using ArkeTest.DTO.Login;
using ArkeTest.Models;
using Microsoft.AspNetCore.Identity;
using System.Net;

namespace ArkeTest.Services.Login
{
    public class CreateLoginService(UserManager<ApplicationUser> userManager, ILogger<CreateLoginService> logger)
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

                IdentityResult? result = await _userManager.CreateAsync(login, dto.Password);
                
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

                else{ 

                    ReturnDTO returnDTO = new()
                    {
                        Message = "Login Created",
                        StatusCode = HttpStatusCode.Created     
                    };
                    _logger.LogInformation("Login created");

                    return returnDTO;
                }
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Internal error creating login");

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
