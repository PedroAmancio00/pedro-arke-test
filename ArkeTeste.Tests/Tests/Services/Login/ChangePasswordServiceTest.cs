using ArkeTest.DTO;
using ArkeTest.Models;
using ArkeTest.Services.Login.ILogin;
using ArkeTest.Services.Login;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using ArkeTest.DTO.Login;

namespace ArkeTeste.Tests.Tests.Services.Login
{
    public class ChangePasswordServiceTest
    {
        [Fact]
        public async Task ChangePasswordService_Password_Success()
        {
            // Mocking services
            Mock<IUserStore<ApplicationUser>> store = new();
#pragma warning disable CS8625 // Cannot convert null literal to non-nullable reference type.
            Mock<UserManager<ApplicationUser>> mockUserManager = new(store.Object, null, null, null, null, null, null, null, null);
#pragma warning restore CS8625 // Cannot convert null literal to non-nullable reference type.
            Mock<ILogger<ChangePasswordService>> mockILogger = new();
            Mock<IJwtService> mockIJwtService = new();
            ChangePasswordService changePasswordService = new(mockUserManager.Object, mockIJwtService.Object, mockILogger.Object);

            ChangePasswordDto dto = new()
            {
                Email = "user@example.com",
                OldPassword = "Password123!",
                NewPassword = "Password12345!"
            };

            // Mocking functions
            mockUserManager.Setup(x => x.FindByEmailAsync(It.IsAny<string>()))
              .ReturnsAsync(new ApplicationUser() { Id = new Guid().ToString(), UserName = "user@example.com" });

            mockUserManager.Setup(x => x.CheckPasswordAsync(It.IsAny<ApplicationUser>(), It.IsAny<string>()))
              .ReturnsAsync(true);

            // Getting result
            ReturnDTO result = await changePasswordService.ChangePassword(dto);

            ReturnDTO returnDTO = new()
            {
                Message = "Password Changed",
                StatusCode = HttpStatusCode.OK
            };

            // Asserting
            Assert.NotNull(result);
            Assert.Equal(returnDTO.Message, result.Message);
            Assert.Equal(returnDTO.StatusCode, result.StatusCode);

            mockUserManager.Verify(x => x.FindByEmailAsync(It.IsAny<string>()), Times.Once);
            mockUserManager.Verify(x => x.CheckPasswordAsync(It.IsAny<ApplicationUser>(), It.IsAny<string>()), Times.Once);
        }

        [Fact]
        public async Task ChangePasswordService_Code_Success()
        {
            // Mocking services
            Mock<IUserStore<ApplicationUser>> store = new();
#pragma warning disable CS8625 // Cannot convert null literal to non-nullable reference type.
            Mock<UserManager<ApplicationUser>> mockUserManager = new(store.Object, null, null, null, null, null, null, null, null);
#pragma warning restore CS8625 // Cannot convert null literal to non-nullable reference type.
            Mock<ILogger<ChangePasswordService>> mockILogger = new();
            Mock<IJwtService> mockIJwtService = new();
            ChangePasswordService changePasswordService = new(mockUserManager.Object, mockIJwtService.Object, mockILogger.Object);

            ChangePasswordDto dto = new()
            {
                Email = "user@example.com",
                NewPassword = "Password12345!",
                RecoveryCode = "1234"
            };

            // Mocking functions
            mockUserManager.Setup(x => x.FindByEmailAsync(It.IsAny<string>()))
              .ReturnsAsync(new ApplicationUser() { Id = new Guid().ToString(), UserName = "user@example.com" });

            // Getting result
            ReturnDTO result = await changePasswordService.ChangePassword(dto);

            ReturnDTO returnDTO = new()
            {
                Message = "Password Changed",
                StatusCode = HttpStatusCode.OK
            };

            // Asserting
            Assert.NotNull(result);
            Assert.Equal(returnDTO.Message, result.Message);
            Assert.Equal(returnDTO.StatusCode, result.StatusCode);

            mockUserManager.Verify(x => x.FindByEmailAsync(It.IsAny<string>()), Times.Once);
        }      


        [Fact]
        public async Task ChangePasswordService_Login_NotFound()
        {
            // Mocking services
            Mock<IUserStore<ApplicationUser>> store = new();
#pragma warning disable CS8625 // Cannot convert null literal to non-nullable reference type.
            Mock<UserManager<ApplicationUser>> mockUserManager = new(store.Object, null, null, null, null, null, null, null, null);
#pragma warning restore CS8625 // Cannot convert null literal to non-nullable reference type.
            Mock<ILogger<ChangePasswordService>> mockILogger = new();
            Mock<IJwtService> mockIJwtService = new();
            ChangePasswordService changePasswordService = new(mockUserManager.Object, mockIJwtService.Object, mockILogger.Object);

            ChangePasswordDto dto = new()
            {
                Email = "user@example.com",
                OldPassword = "Password123!",
                NewPassword = "Password12345!"
            };

            // Mocking functions
            mockUserManager.Setup(x => x.FindByEmailAsync(It.IsAny<string>()))
              .ReturnsAsync(null as ApplicationUser);

            // Getting result
            ReturnDTO result = await changePasswordService.ChangePassword(dto);

            ReturnDTO returnDTO = new()
            {
                Message = "Email or Old Password or Verification Code is wrong",
                StatusCode = HttpStatusCode.NotFound
            };

            // Asserting
            Assert.NotNull(result);
            Assert.Equal(returnDTO.Message, result.Message);
            Assert.Equal(returnDTO.StatusCode, result.StatusCode);

            mockUserManager.Verify(x => x.FindByEmailAsync(It.IsAny<string>()), Times.Once);
        }

        [Fact]
        public async Task ChangePasswordService_NoCodeWrongPassword_NotFound()
        {
            // Mocking services
            Mock<IUserStore<ApplicationUser>> store = new();
#pragma warning disable CS8625 // Cannot convert null literal to non-nullable reference type.
            Mock<UserManager<ApplicationUser>> mockUserManager = new(store.Object, null, null, null, null, null, null, null, null);
#pragma warning restore CS8625 // Cannot convert null literal to non-nullable reference type.
            Mock<ILogger<ChangePasswordService>> mockILogger = new();
            Mock<IJwtService> mockIJwtService = new();
            ChangePasswordService changePasswordService = new(mockUserManager.Object, mockIJwtService.Object, mockILogger.Object);

            ChangePasswordDto dto = new()
            {
                Email = "user@example.com",
                OldPassword = "Password123!",
                NewPassword = "Password12345!"
            };

            // Mocking functions
            mockUserManager.Setup(x => x.FindByEmailAsync(It.IsAny<string>()))
              .ReturnsAsync(new ApplicationUser() { Id = new Guid().ToString(), UserName = "user@example.com" });

            mockUserManager.Setup(x => x.CheckPasswordAsync(It.IsAny<ApplicationUser>(), It.IsAny<string>()))
              .ReturnsAsync(false);

            // Getting result
            ReturnDTO result = await changePasswordService.ChangePassword(dto);

            ReturnDTO returnDTO = new()
            {
                Message = "Email or Old Password or Verification Code is wrong",
                StatusCode = HttpStatusCode.NotFound
            };

            // Asserting
            Assert.NotNull(result);
            Assert.Equal(returnDTO.Message, result.Message);
            Assert.Equal(returnDTO.StatusCode, result.StatusCode);

            mockUserManager.Verify(x => x.FindByEmailAsync(It.IsAny<string>()), Times.Once);
            mockUserManager.Verify(x => x.CheckPasswordAsync(It.IsAny<ApplicationUser>(), It.IsAny<string>()), Times.Once);
        }

    }
}
