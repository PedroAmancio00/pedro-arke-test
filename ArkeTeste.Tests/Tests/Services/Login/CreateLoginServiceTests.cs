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
        public async Task CreateLogin_Success()
        {
            // Mocking services
            Mock<IUserStore<ApplicationUser>> store = new();
#pragma warning disable CS8625 // Cannot convert null literal to non-nullable reference type.
            Mock<UserManager<ApplicationUser>> mockUserManager = new(store.Object, null, null, null, null, null, null, null, null);
#pragma warning restore CS8625 // Cannot convert null literal to non-nullable reference type.
            Mock<ILogger<CreateLoginService>> mockILogger = new();
            CreateLoginService createLoginService = new(mockUserManager.Object, mockILogger.Object);

            CreateLoginDTO dto = new()
            {
                Email = "user@example.com",
                Password = "Password123!"
            };

            // Mocking functions
            mockUserManager.Setup(x => x.CreateAsync(It.IsAny<ApplicationUser>(), It.IsAny<string>()))
                      .ReturnsAsync(IdentityResult.Success);

            // Getting result
            ReturnDTO result = await createLoginService.CreateLogin(dto);

            ReturnDTO returnDTO = new()
            {
                Message = "Login Created",
                StatusCode = HttpStatusCode.Created
            };

            // Asserting
            Assert.NotNull(result);
            Assert.Equal(returnDTO.Message, result.Message);
            Assert.Equal(returnDTO.StatusCode, result.StatusCode);

            mockUserManager.Verify(x => x.CreateAsync(It.IsAny<ApplicationUser>(), It.IsAny<string>()), Times.Once);
        }

        [Fact]
        public async Task CreateLogin_Conflict()
        {
            // Mocking services
            Mock<IUserStore<ApplicationUser>> store = new();
#pragma warning disable CS8625 // Cannot convert null literal to non-nullable reference type.
            Mock<UserManager<ApplicationUser>> mockUserManager = new(store.Object, null, null, null, null, null, null, null, null);
#pragma warning restore CS8625 // Cannot convert null literal to non-nullable reference type.
            Mock<ILogger<CreateLoginService>> mockILogger = new();
            CreateLoginService createLoginService = new(mockUserManager.Object, mockILogger.Object);

            CreateLoginDTO dto = new()
            {
                Email = "user@example.com",
                Password = "Password123!"
            };

            // Mocking functions to fail
            mockUserManager.Setup(x => x.CreateAsync(It.IsAny<ApplicationUser>(), It.IsAny<string>()))
                       .ReturnsAsync(IdentityResult.Failed(new IdentityError { Description = "Error creating login" }));

            // Getting result
            ReturnDTO result = await createLoginService.CreateLogin(dto);

            ReturnDTO returnDTO = new()
            {
                Message = "Error creating login",
                StatusCode = HttpStatusCode.Conflict
            };

            // Asserting
            Assert.NotNull(result);
            Assert.Equal(returnDTO.Message, result.Message);
            Assert.Equal(returnDTO.StatusCode, result.StatusCode);

            mockUserManager.Verify(x => x.CreateAsync(It.IsAny<ApplicationUser>(), It.IsAny<string>()), Times.Once);
        }
    }
}
