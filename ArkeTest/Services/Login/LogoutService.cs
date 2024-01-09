using ArkeTest.DTO;
using ArkeTest.Services.Login.ILogin;
using System.Net;

namespace ArkeTest.Services.Login
{
    public class LogoutService(IJwtService jwtService, ILogger<LogoutService> logger) : ILogoutService
    {
        private readonly IJwtService _jwtService = jwtService;
        private readonly ILogger<LogoutService> _logger = logger;

        public ReturnDTO Logout()
        {
            try
            {
                // Remove tokens from cookies
                _jwtService.RemoveTokens();
                ReturnDTO returnDTO = new()
                {
                    Message = "Logout successfully",
                    StatusCode = HttpStatusCode.OK
                };
                _logger.LogInformation("Logout succesfully");
                // Return a 200
                return returnDTO;
            }
            // If an error occurs, return a 500
            catch (Exception ex)
            {
                _logger.LogError(ex, "Logout failed");
                ReturnDTO returnDTO = new()
                {
                    Message = "Error Login out",
                    StatusCode = HttpStatusCode.InternalServerError
                };

                return returnDTO;
            }
        }
    }
}
