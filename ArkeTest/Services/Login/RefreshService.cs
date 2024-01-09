using ArkeTest.Data;
using ArkeTest.DTO;
using ArkeTest.Models;
using ArkeTest.Services.Login.ILogin;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace ArkeTest.Services.Login
{
    public class RefreshService(MyDbContext db, IJwtService jwtService, ILogger<RefreshService> logger) : IRefreshService
    {
        private readonly MyDbContext _db = db;
        private readonly ILogger<RefreshService> _logger = logger;
        private readonly IJwtService _jwtService = jwtService;

        public async Task<ReturnDTO> Refresh()
        {
            try
            {
                string? token = _jwtService.GetRefreshToken();

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
                    ApplicationUser? login = await _db.ApplicationUsers
                        .FirstOrDefaultAsync(x => x.RefreshToken == token && x.RefreshTokenExpiryTime >= DateTime.UtcNow);

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
