using ArkeTest.Data;
using ArkeTest.DTO;
using ArkeTest.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace ArkeTest.Services.Login
{
    public class RefreshService(MyDbContext db, ILogger<RefreshService> logger, AccessAccountService accessAccountService)
    {
        private readonly MyDbContext _db = db;
        private readonly ILogger<RefreshService> _logger = logger;
        private readonly AccessAccountService _accessAccountService = accessAccountService;

        public async Task<ReturnJwtDTO> Refresh(string refresh)
        {
            try
            {
                ApplicationUser? login = await _db.ApplicationUsers
                    .FirstOrDefaultAsync(x => x.RefreshToken == refresh && x.RefreshTokenExpiryTime >= DateTime.UtcNow);

                if (login == null)
                {

                    ReturnJwtDTO returnDTO = new()
                    {
                        Message = "Refresh failed",
                        StatusCode = HttpStatusCode.NotFound
                    };
                    _logger.LogInformation("Refresh failed");

                    return returnDTO;

                }
                else
                {
#pragma warning disable CS8604 // Possible null reference argument.
                    var token = _accessAccountService.GenerateJwtToken(login.Id, login.UserName);
#pragma warning restore CS8604 // Possible null reference argument.

                    ReturnJwtDTO returnDTO = new()
                    {
                        Message = "Refreshed",
                        StatusCode = HttpStatusCode.OK,
                        JwtToken = token
                    };
                    _logger.LogInformation("Refresh successful");

                    return returnDTO;
                }
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Internal error logging in");

                ReturnJwtDTO returnDTO = new()
                {
                    Message = "Internal error logging in",
                    StatusCode = HttpStatusCode.InternalServerError
                };

                return returnDTO;
            }
        }
    }
}
