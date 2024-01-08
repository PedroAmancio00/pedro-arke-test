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
        [Fact]
        public async Task CreateLogin_ReturnsSuccess()
        {
            // Arrange
            var store = new Mock<IUserStore<ApplicationUser>>();
#pragma warning disable CS8625 // Cannot convert null literal to non-nullable reference type.
            var mockUserManager = new Mock<UserManager<ApplicationUser>>(store.Object, null, null, null, null, null, null, null, null);
#pragma warning restore CS8625 // Cannot convert null literal to non-nullable reference type.
            var mockILogger = new Mock<ILogger<CreateLoginService>>();
            var createLoginService = new CreateLoginService(mockUserManager.Object, mockILogger.Object);

            CreateLoginDTO dto = new()
            {
                Email = "user@example.com",
                Password = "Password123!"
            };

            mockUserManager.Setup(x => x.CreateAsync(It.IsAny<ApplicationUser>(), It.IsAny<string>()))
                      .ReturnsAsync(IdentityResult.Success);

            var result = await createLoginService.CreateLogin(dto);

            ReturnDTO returnDTO = new()
            {
                Message = "Login Created",
                StatusCode = HttpStatusCode.Created
            };

            Assert.NotNull(result);
            Assert.Equal(returnDTO.Message, result.Message);
            Assert.Equal(returnDTO.StatusCode, result.StatusCode);

            mockUserManager.Verify(x => x.CreateAsync(It.IsAny<ApplicationUser>(), It.IsAny<string>()), Times.Once);
        }

        [Fact]
        public async Task CreateLogin_ReturnsConflict()
        {
            // Arrange
            var store = new Mock<IUserStore<ApplicationUser>>();
#pragma warning disable CS8625 // Cannot convert null literal to non-nullable reference type.
            var mockUserManager = new Mock<UserManager<ApplicationUser>>(store.Object, null, null, null, null, null, null, null, null);
#pragma warning restore CS8625 // Cannot convert null literal to non-nullable reference type.
            var mockILogger = new Mock<ILogger<CreateLoginService>>();
            var createLoginService = new CreateLoginService(mockUserManager.Object, mockILogger.Object);

            CreateLoginDTO dto = new()
            {
                Email = "user@example.com",
                Password = "Password123!"
            };

            mockUserManager.Setup(x => x.CreateAsync(It.IsAny<ApplicationUser>(), It.IsAny<string>()))
                       .ReturnsAsync(IdentityResult.Failed(new IdentityError { Description = "Error creating login" }));

            var result = await createLoginService.CreateLogin(dto);

            ReturnDTO returnDTO = new()
            {
                Message = "Error creating login",
                StatusCode = HttpStatusCode.Conflict
            };

            Assert.NotNull(result);
            Assert.Equal(returnDTO.Message, result.Message);
            Assert.Equal(returnDTO.StatusCode, result.StatusCode);

            mockUserManager.Verify(x => x.CreateAsync(It.IsAny<ApplicationUser>(), It.IsAny<string>()), Times.Once);
        }
    }
}
