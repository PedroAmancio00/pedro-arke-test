using ArkeTest.Data;
using ArkeTest.DTO;
using Microsoft.AspNetCore.Identity;
using System.Net;

namespace ArkeTest.Services.Login
{
    public class CreateLoginService
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly ILogger<CreateLoginService> _logger;

        public CreateLoginService(UserManager<IdentityUser> userManager, ILogger<CreateLoginService> logger)
        {
            _userManager = userManager;
            _logger = logger;
        }

        public async Task<ReturnDTO> CreateLogin(CreateLoginDTO dto)
        {
            try
            {
                IdentityUser login = new()
                {
                    Email = dto.Email,
                    UserName = dto.Email,
                    PasswordHash = dto.Password
                };

                var result = await _userManager.CreateAsync(login);
                
                if (!result.Succeeded)
                {
                    string message = result.Errors.Select(e => e.Description).Aggregate((a, b) => $"{a}, {b}");

                    ReturnDTO erorReturnDTO = new()
                    {
                        Message = message,
                        StatusCode = HttpStatusCode.Conflict
                    };
                    _logger.LogInformation("Error creating login");
                    return erorReturnDTO;
                }

                ReturnDTO returnDTO = new()
                {
                    Message = "Login Created",
                    StatusCode = HttpStatusCode.Created     
                };
                _logger.LogInformation("Login created");

                return returnDTO;

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
