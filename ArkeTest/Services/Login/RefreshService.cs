using ArkeTest.Data;
using ArkeTest.DTO;
using ArkeTest.Models;
using ArkeTest.Services.Login.ILogin;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace ArkeTest.Services.Login
{
    public class RefreshService : IRefreshService
    {
        private readonly MyDbContext _db;
        private readonly ILogger<RefreshService> _logger;
        private readonly IJwtService _jwtService;

        public RefreshService(MyDbContext db, IJwtService jwtService, ILogger<RefreshService> logger)
        {
            _db = db;
            _jwtService = jwtService;
            _logger = logger;
        }

        public async Task<ReturnDTO> Refresh()
        {
            try
            {
                // Get the refresh token from the cookie
                string? token = _jwtService.GetRefreshToken();

                // If the token is null, return a 404
                if (token == null)
                {
                    ReturnDTO returnDTO = new()
                    {
                        Message = "Refresh failed",
                        StatusCode = HttpStatusCode.NotFound
                    };
                    _logger.LogInformation("Refresh failed");

                    return returnDTO;
                }
                else
                {
                    // Find the user with the refresh token
                    ApplicationUser? login = await _db.ApplicationUsers
                        .FirstOrDefaultAsync(x => x.RefreshToken == token && x.RefreshTokenExpiryTime >= DateTime.UtcNow);

                    // If the user is null, return a 404
                    if (login == null)
                    {

                        ReturnDTO returnDTO = new()
                        {
                            Message = "Refresh failed",
                            StatusCode = HttpStatusCode.NotFound
                        };
                        _logger.LogInformation("Refresh failed");

                        return returnDTO;

                    }
                    else
                    {
                        // Generate a new JWT token
                        _jwtService.GenerateJwtToken(login);

                        ReturnDTO returnDTO = new()
                        {
                            Message = "Refreshed",
                            StatusCode = HttpStatusCode.OK
                        };
                        _logger.LogInformation("Refresh successful");

                        return returnDTO;
                    }
                }
            }
            // If there is an exception, return a 500
            catch (Exception ex)
            {
                _logger.LogError(ex, "Internal error logging in");

                ReturnDTO returnDTO = new()
                {
                    Message = "Internal error logging in",
                    StatusCode = HttpStatusCode.InternalServerError
                };

                return returnDTO;
            }
        }
    }
}
