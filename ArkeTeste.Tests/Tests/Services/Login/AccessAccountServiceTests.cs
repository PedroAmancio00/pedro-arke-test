using ArkeTest.DTO;
using ArkeTest.Models;
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
        public async Task AccessAccount_ReturnsSuccess()
        {
            // Arrange
            var store = new Mock<IUserStore<ApplicationUser>>();
#pragma warning disable CS8625 // Cannot convert null literal to non-nullable reference type.
            var mockUserManager = new Mock<UserManager<ApplicationUser>>(store.Object, null, null, null, null, null, null, null, null);
#pragma warning restore CS8625 // Cannot convert null literal to non-nullable reference type.
            var mockILogger = new Mock<ILogger<AccessAccountService>>();
            var accessAccountService = new AccessAccountService(mockUserManager.Object, mockILogger.Object);

            AccessAccountDTO dto = new()
            {
                Email = "user@example.com",
                Password = "Password123!"
            };

            mockUserManager.Setup(x => x.FindByEmailAsync(It.IsAny<string>()))
              .ReturnsAsync(new ApplicationUser(){ Id = new Guid().ToString(), UserName = "test@test.com"});

            mockUserManager.Setup(x => x.CheckPasswordAsync(It.IsAny<ApplicationUser>(), It.IsAny<string>()))
              .ReturnsAsync(true);

            mockUserManager.Setup(x => x.UpdateAsync(It.IsAny<ApplicationUser>()))
              .ReturnsAsync(IdentityResult.Success);



            var result = await accessAccountService.AccessAccount(dto);

            ReturnDTO returnDTO = new()
            {
                Message = "Successful Login",
                StatusCode = HttpStatusCode.OK
            };

            Assert.NotNull(result);
            Assert.Equal(returnDTO.Message, result.Message);
            Assert.Equal(returnDTO.StatusCode, result.StatusCode);

            mockUserManager.Verify(x => x.FindByEmailAsync(It.IsAny<string>()), Times.Once);
            mockUserManager.Verify(x => x.CheckPasswordAsync(It.IsAny<ApplicationUser>(), It.IsAny<string>()), Times.Once);
            mockUserManager.Verify(x => x.UpdateAsync(It.IsAny<ApplicationUser>()), Times.Once);
        }


        [Fact]
        public async Task AccessAccount_ReturnsConflictEmail()
        {
            // Arrange
            var store = new Mock<IUserStore<ApplicationUser>>();
#pragma warning disable CS8625 // Cannot convert null literal to non-nullable reference type.
            var mockUserManager = new Mock<UserManager<ApplicationUser>>(store.Object, null, null, null, null, null, null, null, null);
#pragma warning restore CS8625 // Cannot convert null literal to non-nullable reference type.
            var mockILogger = new Mock<ILogger<AccessAccountService>>();
            var accessAccountService = new AccessAccountService(mockUserManager.Object, mockILogger.Object);

            AccessAccountDTO dto = new()
            {
                Email = "user@example.com",
                Password = "Password123!"
            };

            mockUserManager.Setup(x => x.FindByEmailAsync(It.IsAny<string>()))
              .ReturnsAsync(null as ApplicationUser);

            var result = await accessAccountService.AccessAccount(dto);

            ReturnDTO returnDTO = new()
            {
                Message = "Email or Password is wrong",
                StatusCode = HttpStatusCode.Conflict
            };

            Assert.NotNull(result);
            Assert.Equal(returnDTO.Message, result.Message);
            Assert.Equal(returnDTO.StatusCode, result.StatusCode);

            mockUserManager.Verify(x => x.FindByEmailAsync(It.IsAny<string>()), Times.Once);

        }


        [Fact]
        public async Task AccessAccount_ReturnsConflictPassword()
        {
            // Arrange
            var store = new Mock<IUserStore<ApplicationUser>>();
#pragma warning disable CS8625 // Cannot convert null literal to non-nullable reference type.
            var mockUserManager = new Mock<UserManager<ApplicationUser>>(store.Object, null, null, null, null, null, null, null, null);
#pragma warning restore CS8625 // Cannot convert null literal to non-nullable reference type.
            var mockILogger = new Mock<ILogger<AccessAccountService>>();
            var accessAccountService = new AccessAccountService(mockUserManager.Object, mockILogger.Object);

            AccessAccountDTO dto = new()
            {
                Email = "user@example.com",
                Password = "Password123!"
            };

            mockUserManager.Setup(x => x.FindByEmailAsync(It.IsAny<string>()))
             .ReturnsAsync(new ApplicationUser() { Id = new Guid().ToString(), UserName = "test@test.com" });

            mockUserManager.Setup(x => x.CheckPasswordAsync(It.IsAny<ApplicationUser>(), It.IsAny<string>()))
              .ReturnsAsync(false);

            var result = await accessAccountService.AccessAccount(dto);

            ReturnDTO returnDTO = new()
            {
                Message = "Email or Password is wrong",
                StatusCode = HttpStatusCode.Conflict
            };

            Assert.NotNull(result);
            Assert.Equal(returnDTO.Message, result.Message);
            Assert.Equal(returnDTO.StatusCode, result.StatusCode);

            mockUserManager.Verify(x => x.FindByEmailAsync(It.IsAny<string>()), Times.Once);
            mockUserManager.Verify(x => x.CheckPasswordAsync(It.IsAny<ApplicationUser>(), It.IsAny<string>()), Times.Once);
        }


        [Fact]
        public async Task AccessAccount_GenerateJwtAndRefreshToken()
        {
            // Arrange
            var store = new Mock<IUserStore<ApplicationUser>>();
#pragma warning disable CS8625 // Cannot convert null literal to non-nullable reference type.
            var mockUserManager = new Mock<UserManager<ApplicationUser>>(store.Object, null, null, null, null, null, null, null, null);
#pragma warning restore CS8625 // Cannot convert null literal to non-nullable reference type.
            var mockILogger = new Mock<ILogger<AccessAccountService>>();
            var accessAccountService = new AccessAccountService(mockUserManager.Object, mockILogger.Object);

            ApplicationUser applicationUser = new() { Id = new Guid().ToString(), UserName = "test@test.com" };

            mockUserManager.Setup(x => x.UpdateAsync(It.IsAny<ApplicationUser>()))
              .ReturnsAsync(IdentityResult.Success);

            var result = await accessAccountService.GenerateJwtAndRefreshToken(applicationUser);

            Assert.NotNull(result.Item1);
            Assert.NotNull(result.Item2);
            mockUserManager.Verify(x => x.UpdateAsync(It.IsAny<ApplicationUser>()), Times.Once);
        }

        [Fact]
        public void AccessAccount_GenerateJwtToken()
        {
            // Arrange
            var store = new Mock<IUserStore<ApplicationUser>>();
#pragma warning disable CS8625 // Cannot convert null literal to non-nullable reference type.
            var mockUserManager = new Mock<UserManager<ApplicationUser>>(store.Object, null, null, null, null, null, null, null, null);
#pragma warning restore CS8625 // Cannot convert null literal to non-nullable reference type.
            var mockILogger = new Mock<ILogger<AccessAccountService>>();
            var accessAccountService = new AccessAccountService(mockUserManager.Object, mockILogger.Object);

            ApplicationUser applicationUser = new() { Id = new Guid().ToString(), UserName = "test@test.com" };

            var result = accessAccountService.GenerateJwtToken(applicationUser.Id, applicationUser.UserName);

            Assert.NotNull(result);
        }

        [Fact]
        public async Task AccessAccount_SaveRefreshToken()
        {
            // Arrange
            var store = new Mock<IUserStore<ApplicationUser>>();
#pragma warning disable CS8625 // Cannot convert null literal to non-nullable reference type.
            var mockUserManager = new Mock<UserManager<ApplicationUser>>(store.Object, null, null, null, null, null, null, null, null);
#pragma warning restore CS8625 // Cannot convert null literal to non-nullable reference type.
            var mockILogger = new Mock<ILogger<AccessAccountService>>();
            var accessAccountService = new AccessAccountService(mockUserManager.Object, mockILogger.Object);

            ApplicationUser applicationUser = new() { Id = new Guid().ToString(), UserName = "test@test.com" };

            mockUserManager.Setup(x => x.UpdateAsync(It.IsAny<ApplicationUser>()))
             .ReturnsAsync(IdentityResult.Success);

            var result = await accessAccountService.SaveRefreshToken(applicationUser, "test");

            Assert.True(result);

            mockUserManager.Verify(x => x.UpdateAsync(It.IsAny<ApplicationUser>()), Times.Once);
        }
    }
}
