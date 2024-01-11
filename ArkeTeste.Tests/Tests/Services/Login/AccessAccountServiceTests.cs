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
        private readonly AccessAccountService _accessAccountService;
        private readonly Mock<IUserStore<ApplicationUser>> _store;
        private readonly Mock<UserManager<ApplicationUser>> _mockUserManager;
        private readonly Mock<ILogger<AccessAccountService>> _mockILogger;
        private readonly Mock<IJwtService> _mockIJwtService;

        public AccessAccountServiceTests()
        {
            _store = new Mock<IUserStore<ApplicationUser>>();
#pragma warning disable CS8625 // Cannot convert null literal to non-nullable reference type.
            _mockUserManager = new Mock<UserManager<ApplicationUser>>(_store.Object, null, null, null, null, null, null, null, null);
#pragma warning restore CS8625 // Cannot convert null literal to non-nullable reference type.
            _mockILogger = new Mock<ILogger<AccessAccountService>>();
            _mockIJwtService = new Mock<IJwtService>();

            _accessAccountService = new(_mockUserManager.Object, _mockIJwtService.Object, _mockILogger.Object);
        }

        [Fact]
        public async Task AccessAccount_Success()
        {
            AccessAccountDTO dto = new()
            {
                Email = "user@example.com",
                Password = "Password123!"
            };

            // Mocking functions
            _mockUserManager.Setup(x => x.FindByEmailAsync(It.IsAny<string>()))
              .ReturnsAsync(new ApplicationUser() { Id = new Guid().ToString(), UserName = "test@test.com" });

            _mockUserManager.Setup(x => x.CheckPasswordAsync(It.IsAny<ApplicationUser>(), It.IsAny<string>()))
              .ReturnsAsync(true);

            // Getting result
            ReturnDTO result = await _accessAccountService.AccessAccount(dto);

            ReturnDTO returnDTO = new()
            {
                Message = "Successful Login",
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
        public async Task AccessAccount_Email_Conflict()
        {
            AccessAccountDTO dto = new()
            {
                Email = "user@example.com",
                Password = "Password123!"
            };

            // Mocking functions to fail
            _mockUserManager.Setup(x => x.FindByEmailAsync(It.IsAny<string>()))
              .ReturnsAsync(null as ApplicationUser);

            // Getting result
            ReturnDTO result = await _accessAccountService.AccessAccount(dto);

            ReturnDTO returnDTO = new()
            {
                Message = "Email or Password is wrong",
                StatusCode = HttpStatusCode.Conflict
            };

            // Asserting
            Assert.NotNull(result);
            Assert.Equal(returnDTO.Message, result.Message);
            Assert.Equal(returnDTO.StatusCode, result.StatusCode);

            _mockUserManager.Verify(x => x.FindByEmailAsync(It.IsAny<string>()), Times.Once);

        }


        [Fact]
        public async Task AccessAccount_Password_Conflict()
        {
            AccessAccountDTO dto = new()
            {
                Email = "user@example.com",
                Password = "Password123!"
            };

            // Mocking functions
            _mockUserManager.Setup(x => x.FindByEmailAsync(It.IsAny<string>()))
             .ReturnsAsync(new ApplicationUser() { Id = new Guid().ToString(), UserName = "test@test.com" });

            // Mocking functions to fail
            _mockUserManager.Setup(x => x.CheckPasswordAsync(It.IsAny<ApplicationUser>(), It.IsAny<string>()))
              .ReturnsAsync(false);

            // Getting result
            ReturnDTO result = await _accessAccountService.AccessAccount(dto);

            ReturnDTO returnDTO = new()
            {
                Message = "Email or Password is wrong",
                StatusCode = HttpStatusCode.Conflict
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
