using ArkeTest.DTO;
using ArkeTest.DTO.Login;
using ArkeTest.Models;
using ArkeTest.Services.Jwt.IJwt;
using ArkeTest.Services.Login;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Moq;
using System.Net;

namespace ArkeTeste.Tests.Tests.Services.Login
{
    public class ChangePasswordServiceTest
    {
        private readonly ChangePasswordService _changePasswordService;
        private readonly Mock<IUserStore<ApplicationUser>> _store;
        private readonly Mock<UserManager<ApplicationUser>> _mockUserManager;
        private readonly Mock<ILogger<ChangePasswordService>> _mockILogger;
        private readonly Mock<IJwtService> _mockIJwtService;

        public ChangePasswordServiceTest()
        {
            _store = new Mock<IUserStore<ApplicationUser>>();
#pragma warning disable CS8625 // Cannot convert null literal to non-nullable reference type.
            _mockUserManager = new Mock<UserManager<ApplicationUser>>(_store.Object, null, null, null, null, null, null, null, null);
#pragma warning restore CS8625 // Cannot convert null literal to non-nullable reference type.
            _mockILogger = new Mock<ILogger<ChangePasswordService>>();
            _mockIJwtService = new Mock<IJwtService>();

            _changePasswordService = new(_mockUserManager.Object, _mockIJwtService.Object, _mockILogger.Object);
        }

        [Fact]
        public async Task ChangePasswordService_Password_Success()
        {
            ChangePasswordDto dto = new()
            {
                Email = "user@example.com",
                OldPassword = "Password123!",
                NewPassword = "Password12345!"
            };

            // Mocking functions
            _mockUserManager.Setup(x => x.FindByEmailAsync(It.IsAny<string>()))
              .ReturnsAsync(new ApplicationUser() { Id = new Guid().ToString(), UserName = "user@example.com" });

            _mockUserManager.Setup(x => x.CheckPasswordAsync(It.IsAny<ApplicationUser>(), It.IsAny<string>()))
              .ReturnsAsync(true);

            // Getting result
            ReturnDTO result = await _changePasswordService.ChangePassword(dto);

            ReturnDTO returnDTO = new()
            {
                Message = "Password Changed",
                StatusCode = HttpStatusCode.OK
            };

            // Asserting
            Assert.NotNull(result);
            Assert.Equal(returnDTO.Message, result.Message);
            Assert.Equal(returnDTO.StatusCode, result.StatusCode);

            _mockUserManager.Verify(x => x.FindByEmailAsync(It.IsAny<string>()), Times.Once);
            _mockUserManager.Verify(x => x.CheckPasswordAsync(It.IsAny<ApplicationUser>(), It.IsAny<string>()), Times.Once);
        }

        [Fact]
        public async Task ChangePasswordService_Code_Success()
        {
            ChangePasswordDto dto = new()
            {
                Email = "user@example.com",
                NewPassword = "Password12345!",
                RecoveryCode = "1234"
            };

            // Mocking functions
            _mockUserManager.Setup(x => x.FindByEmailAsync(It.IsAny<string>()))
              .ReturnsAsync(new ApplicationUser() { Id = new Guid().ToString(), UserName = "user@example.com" });

            // Getting result
            ReturnDTO result = await _changePasswordService.ChangePassword(dto);

            ReturnDTO returnDTO = new()
            {
                Message = "Password Changed",
                StatusCode = HttpStatusCode.OK
            };

            // Asserting
            Assert.NotNull(result);
            Assert.Equal(returnDTO.Message, result.Message);
            Assert.Equal(returnDTO.StatusCode, result.StatusCode);

            _mockUserManager.Verify(x => x.FindByEmailAsync(It.IsAny<string>()), Times.Once);
        }


        [Fact]
        public async Task ChangePasswordService_Login_NotFound()
        {
            ChangePasswordDto dto = new()
            {
                Email = "user@example.com",
                OldPassword = "Password123!",
                NewPassword = "Password12345!"
            };

            // Mocking functions
            _mockUserManager.Setup(x => x.FindByEmailAsync(It.IsAny<string>()))
              .ReturnsAsync(null as ApplicationUser);

            // Getting result
            ReturnDTO result = await _changePasswordService.ChangePassword(dto);

            ReturnDTO returnDTO = new()
            {
                Message = "Email or Old Password or Verification Code is wrong",
                StatusCode = HttpStatusCode.NotFound
            };

            // Asserting
            Assert.NotNull(result);
            Assert.Equal(returnDTO.Message, result.Message);
            Assert.Equal(returnDTO.StatusCode, result.StatusCode);

            _mockUserManager.Verify(x => x.FindByEmailAsync(It.IsAny<string>()), Times.Once);
        }

        [Fact]
        public async Task ChangePasswordService_NoCodeWrongPassword_NotFound()
        {
            ChangePasswordDto dto = new()
            {
                Email = "user@example.com",
                OldPassword = "Password123!",
                NewPassword = "Password12345!"
            };

            // Mocking functions
            _mockUserManager.Setup(x => x.FindByEmailAsync(It.IsAny<string>()))
              .ReturnsAsync(new ApplicationUser() { Id = new Guid().ToString(), UserName = "user@example.com" });

            _mockUserManager.Setup(x => x.CheckPasswordAsync(It.IsAny<ApplicationUser>(), It.IsAny<string>()))
              .ReturnsAsync(false);

            // Getting result
            ReturnDTO result = await _changePasswordService.ChangePassword(dto);

            ReturnDTO returnDTO = new()
            {
                Message = "Email or Old Password or Verification Code is wrong",
                StatusCode = HttpStatusCode.NotFound
            };

            // Asserting
            Assert.NotNull(result);
            Assert.Equal(returnDTO.Message, result.Message);
            Assert.Equal(returnDTO.StatusCode, result.StatusCode);

            _mockUserManager.Verify(x => x.FindByEmailAsync(It.IsAny<string>()), Times.Once);
            _mockUserManager.Verify(x => x.CheckPasswordAsync(It.IsAny<ApplicationUser>(), It.IsAny<string>()), Times.Once);
        }

    }
}
