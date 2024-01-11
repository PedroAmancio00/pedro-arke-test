using ArkeTest.DTO;
using ArkeTest.DTO.Login;
using ArkeTest.Models;
using ArkeTest.Services.Login;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Moq;
using System.Net;

namespace ArkeTest.Tests.Services.Login
{
    public class CreateLoginServiceTests
    {
        private readonly CreateLoginService _createLoginService;
        private readonly Mock<IUserStore<ApplicationUser>> _store;
        private readonly Mock<UserManager<ApplicationUser>> _mockUserManager;
        private readonly Mock<ILogger<CreateLoginService>> _mockILogger;

        public CreateLoginServiceTests()
        {
            _store = new Mock<IUserStore<ApplicationUser>>();
#pragma warning disable CS8625 // Cannot convert null literal to non-nullable reference type.
            _mockUserManager = new Mock<UserManager<ApplicationUser>>(_store.Object, null, null, null, null, null, null, null, null);
#pragma warning restore CS8625 // Cannot convert null literal to non-nullable reference type.
            _mockILogger = new Mock<ILogger<CreateLoginService>>();

            _createLoginService = new(_mockUserManager.Object, _mockILogger.Object);
        }

        [Fact]
        public async Task CreateLogin_Success()
        {
            CreateLoginDTO dto = new()
            {
                Email = "user@example.com",
                Password = "Password123!"
            };

            // Mocking functions
            _mockUserManager.Setup(x => x.CreateAsync(It.IsAny<ApplicationUser>(), It.IsAny<string>()))
                      .ReturnsAsync(IdentityResult.Success);

            // Getting result
            ReturnDTO result = await _createLoginService.CreateLogin(dto);

            ReturnDTO returnDTO = new()
            {
                Message = "Login Created",
                StatusCode = HttpStatusCode.Created
            };

            // Asserting
            Assert.NotNull(result);
            Assert.Equal(returnDTO.Message, result.Message);
            Assert.Equal(returnDTO.StatusCode, result.StatusCode);

            _mockUserManager.Verify(x => x.CreateAsync(It.IsAny<ApplicationUser>(), It.IsAny<string>()), Times.Once);
        }

        [Fact]
        public async Task CreateLogin_Conflict()
        {
            CreateLoginDTO dto = new()
            {
                Email = "user@example.com",
                Password = "Password123!"
            };

            // Mocking functions to fail
            _mockUserManager.Setup(x => x.CreateAsync(It.IsAny<ApplicationUser>(), It.IsAny<string>()))
                       .ReturnsAsync(IdentityResult.Failed(new IdentityError { Description = "Error creating login" }));

            // Getting result
            ReturnDTO result = await _createLoginService.CreateLogin(dto);

            ReturnDTO returnDTO = new()
            {
                Message = "Error creating login",
                StatusCode = HttpStatusCode.Conflict
            };

            // Asserting
            Assert.NotNull(result);
            Assert.Equal(returnDTO.Message, result.Message);
            Assert.Equal(returnDTO.StatusCode, result.StatusCode);

            _mockUserManager.Verify(x => x.CreateAsync(It.IsAny<ApplicationUser>(), It.IsAny<string>()), Times.Once);
        }
    }
}
