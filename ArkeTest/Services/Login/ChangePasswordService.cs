using ArkeTest.DTO;
using ArkeTest.DTO.Login;
using ArkeTest.Models;
using ArkeTest.Services.Jwt.IJwt;
using ArkeTest.Services.Login.ILogin;
using Microsoft.AspNetCore.Identity;
using System.Net;

namespace ArkeTest.Services.Login
{

    public class ChangePasswordService : IChangePasswordService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IJwtService _jwtService;
        private readonly ILogger<ChangePasswordService> _logger;

        public ChangePasswordService(UserManager<ApplicationUser> userManager, IJwtService jwtService, ILogger<ChangePasswordService> logger)
        {
            _userManager = userManager;
            _jwtService = jwtService;
            _logger = logger;
        }

        public async Task<ReturnDTO> ChangePassword(ChangePasswordDto dto)
        {
            try
            {
                string email = dto.Email!;

                // Find the user with the email
                ApplicationUser? login = await _userManager.FindByEmailAsync(email);

                // If the user is null, return a 404
                if (login == null)
                {
                    ReturnDTO returnDTO = new()
                    {
                        Message = "Email or Old Password or Verification Code is wrong",
                        StatusCode = HttpStatusCode.NotFound,
                    };
                    _logger.LogInformation("Login not found");

                    return returnDTO;
                }
                else
                {
                    bool isVerificationCodeCorrect = dto.RecoveryCode == "1234";
                    bool isPasswordCorrect = false;
                    // Check if the verification code is correct and if the old password is not null
                    if (!isVerificationCodeCorrect && dto.OldPassword != null)
                    {
                        // Check if the password is correct
                        isPasswordCorrect = await _userManager.CheckPasswordAsync(login, dto.OldPassword);
                    }

                    // If the password is incorrect and the verification code is incorrect, return a 404
                    if (!isPasswordCorrect && !isVerificationCodeCorrect)
                    {
                        ReturnDTO returnDTO = new()
                        {
                            Message = "Email or Old Password or Verification Code is wrong",
                            StatusCode = HttpStatusCode.NotFound
                        };
                        _logger.LogInformation("Wrong password or recovery code");

                        return returnDTO;
                    }

                    // If everything is correct, change the password
                    else
                    {
                        string token = await _userManager.GeneratePasswordResetTokenAsync(login);
                        IdentityResult result = await _userManager.ResetPasswordAsync(login, token, dto.NewPassword);

                        await _jwtService.GenerateRefreshToken(login);

                        ReturnDTO returnDTO = new()
                        {
                            Message = "Password Changed",
                            StatusCode = HttpStatusCode.OK
                        };
                        _logger.LogInformation("Password Changed");

                        return returnDTO;
                    }

                }

            }

            // If there is an error, return a 500
            catch (Exception ex)
            {
                _logger.LogError(ex, "Internal error on password change");

                ReturnDTO returnDTO = new()
                {
                    Message = "Error on password change",
                    StatusCode = HttpStatusCode.InternalServerError
                };

                return returnDTO;
            }
        }

    }

}
