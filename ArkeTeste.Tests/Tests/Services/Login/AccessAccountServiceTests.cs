using ArkeTest.DTO;
using ArkeTest.Models;
using ArkeTest.Services.Jwt.IJwt;
using ArkeTest.Services.Login;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Moq;
using System.Net;

namespace ArkeTeste.Tests.Tests.Services.Login
{
    public class AccessAccountServiceTests
    {
        [Fact]
        public async Task AccessAccount_Success()
        {
            // Mocking services
            Mock<IUserStore<ApplicationUser>> store = new();
#pragma warning disable CS8625 // Cannot convert null literal to non-nullable reference type.
            Mock<UserManager<ApplicationUser>> mockUserManager = new(store.Object, null, null, null, null, null, null, null, null);
#pragma warning restore CS8625 // Cannot convert null literal to non-nullable reference type.
            Mock<ILogger<AccessAccountService>> mockILogger = new();
            Mock<IJwtService> mockIJwtService = new();
            AccessAccountService accessAccountService = new(mockUserManager.Object, mockIJwtService.Object, mockILogger.Object);

            AccessAccountDTO dto = new()
            {
                Email = "user@example.com",
                Password = "Password123!"
            };

            // Mocking functions
            mockUserManager.Setup(x => x.FindByEmailAsync(It.IsAny<string>()))
              .ReturnsAsync(new ApplicationUser() { Id = new Guid().ToString(), UserName = "test@test.com" });

            mockUserManager.Setup(x => x.CheckPasswordAsync(It.IsAny<ApplicationUser>(), It.IsAny<string>()))
              .ReturnsAsync(true);

            // Getting result
            ReturnDTO result = await accessAccountService.AccessAccount(dto);

            ReturnDTO returnDTO = new()
            {
                Message = "Successful Login",
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
        public async Task AccessAccount_Email_Conflict()
        {
            // Mocking services
            Mock<IUserStore<ApplicationUser>> store = new();
#pragma warning disable CS8625 // Cannot convert null literal to non-nullable reference type.
            Mock<UserManager<ApplicationUser>> mockUserManager = new(store.Object, null, null, null, null, null, null, null, null);
#pragma warning restore CS8625 // Cannot convert null literal to non-nullable reference type.
            Mock<ILogger<AccessAccountService>> mockILogger = new();
            Mock<IJwtService> mockIJwtService = new();
            AccessAccountService accessAccountService = new(mockUserManager.Object, mockIJwtService.Object, mockILogger.Object);

            AccessAccountDTO dto = new()
            {
                Email = "user@example.com",
                Password = "Password123!"
            };

            // Mocking functions to fail
            mockUserManager.Setup(x => x.FindByEmailAsync(It.IsAny<string>()))
              .ReturnsAsync(null as ApplicationUser);

            // Getting result
            ReturnDTO result = await accessAccountService.AccessAccount(dto);

            ReturnDTO returnDTO = new()
            {
                Message = "Email or Password is wrong",
                StatusCode = HttpStatusCode.Conflict
            };

            // Asserting
            Assert.NotNull(result);
            Assert.Equal(returnDTO.Message, result.Message);
            Assert.Equal(returnDTO.StatusCode, result.StatusCode);

            mockUserManager.Verify(x => x.FindByEmailAsync(It.IsAny<string>()), Times.Once);

        }


        [Fact]
        public async Task AccessAccount_Password_Conflict()
        {
            // Mocking services
            Mock<IUserStore<ApplicationUser>> store = new();
#pragma warning disable CS8625 // Cannot convert null literal to non-nullable reference type.
            Mock<UserManager<ApplicationUser>> mockUserManager = new(store.Object, null, null, null, null, null, null, null, null);
#pragma warning restore CS8625 // Cannot convert null literal to non-nullable reference type.
            Mock<ILogger<AccessAccountService>> mockILogger = new();
            Mock<IJwtService> mockIJwtService = new();
            AccessAccountService accessAccountService = new(mockUserManager.Object, mockIJwtService.Object, mockILogger.Object);

            AccessAccountDTO dto = new()
            {
                Email = "user@example.com",
                Password = "Password123!"
            };

            // Mocking functions
            mockUserManager.Setup(x => x.FindByEmailAsync(It.IsAny<string>()))
             .ReturnsAsync(new ApplicationUser() { Id = new Guid().ToString(), UserName = "test@test.com" });

            // Mocking functions to fail
            mockUserManager.Setup(x => x.CheckPasswordAsync(It.IsAny<ApplicationUser>(), It.IsAny<string>()))
              .ReturnsAsync(false);

            // Getting result
            ReturnDTO result = await accessAccountService.AccessAccount(dto);

            ReturnDTO returnDTO = new()
            {
                Message = "Email or Password is wrong",
                StatusCode = HttpStatusCode.Conflict
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
