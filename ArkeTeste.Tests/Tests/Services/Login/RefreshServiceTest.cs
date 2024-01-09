using ArkeTest.Data;
using ArkeTest.DTO;
using ArkeTest.Models;
using ArkeTest.Services.Login;
using ArkeTest.Services.Login.ILogin;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;

namespace ArkeTeste.Tests.Tests.Services
{
    public class RefreshServiceTest
    {
        [Fact]
        public async Task RefreshService_Refresh_Success()
        {
            // Declaring test database
            DbContextOptions<MyDbContext> options = new DbContextOptionsBuilder<MyDbContext>()
                .UseInMemoryDatabase(databaseName: "RefreshService_Refresh_Success") // Make sure to use a unique name for each test
                .Options;
            MyDbContext mockMyDbContext = new(options);

            // Adding data on database
            List<ApplicationUser> testData = new()
            {
                new ApplicationUser { RefreshToken = "token", RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(7) }
            };

            mockMyDbContext.ApplicationUsers.AddRange(testData);
            mockMyDbContext.SaveChanges();

            // Mocking services
            Mock<ILogger<RefreshService>> mockILogger = new();
            Mock<IJwtService> mockIJwtService = new();
            RefreshService refreshService = new(mockMyDbContext, mockIJwtService.Object, mockILogger.Object);

            // Mocking functions
            mockIJwtService.Setup(s => s.GetRefreshToken()).Returns("token");

            mockIJwtService.Setup(s => s.GenerateJwtToken(It.IsAny<ApplicationUser>())).Verifiable();

            // Getting result
            ReturnDTO res = await refreshService.Refresh();

            ReturnDTO returnDTO = new()
            {
                Message = "Refreshed",
                StatusCode = System.Net.HttpStatusCode.OK
            };

            // Asserting
            Assert.NotNull(res);
            Assert.Equal(returnDTO.Message, res.Message);
            Assert.Equal(returnDTO.StatusCode, res.StatusCode);
        }

        [Fact]
        public async Task RefreshService_Refresh_Token_NotFound()
        {
            // Declaring test database
            DbContextOptions<MyDbContext> options = new DbContextOptionsBuilder<MyDbContext>()
                .UseInMemoryDatabase(databaseName: "RefreshService_Refresh_Token_NotFound") // Make sure to use a unique name for each test
                .Options;

            MyDbContext mockMyDbContext = new(options);

            // Mocking services
            Mock<ILogger<RefreshService>> mockILogger = new();
            Mock<IJwtService> mockIJwtService = new();
            Mock<IResponseCookies> mockCookieCollection = new();
            RefreshService refreshService = new(mockMyDbContext, mockIJwtService.Object, mockILogger.Object);

            // Nullifying token for failure
            string? token = null;

            // Mocking functions
            mockIJwtService.Setup(s => s.GetRefreshToken()).Returns(token);

            // Getting result
            ReturnDTO res = await refreshService.Refresh();

            ReturnDTO returnDTO = new()
            {
                Message = "Refresh failed",
                StatusCode = System.Net.HttpStatusCode.NotFound
            };

            // Asserting
            Assert.NotNull(res);
            Assert.Equal(returnDTO.Message, res.Message);
            Assert.Equal(returnDTO.StatusCode, res.StatusCode);
        }

        [Fact]
        public async Task RefreshService_Refresh_Login_NotFound()
        {
            // Declaring test database
            DbContextOptions<MyDbContext> options = new DbContextOptionsBuilder<MyDbContext>()
                .UseInMemoryDatabase(databaseName: "RefreshService_Refresh_Login_NotFound") // Make sure to use a unique name for each test
                .Options;

            MyDbContext mockMyDbContext = new(options);

            // Mocking services
            Mock<ILogger<RefreshService>> mockILogger = new();
            Mock<IJwtService> mockIJwtService = new();
            Mock<IResponseCookies> mockCookieCollection = new();
            RefreshService refreshService = new(mockMyDbContext, mockIJwtService.Object, mockILogger.Object);

            string? token = "token";

            // Mocking functions
            mockIJwtService.Setup(s => s.GetRefreshToken()).Returns(token);

            // Getting result
            ReturnDTO res = await refreshService.Refresh();

            ReturnDTO returnDTO = new()
            {
                Message = "Refresh failed",
                StatusCode = System.Net.HttpStatusCode.NotFound
            };

            // Asserting
            Assert.NotNull(res);
            Assert.Equal(returnDTO.Message, res.Message);
            Assert.Equal(returnDTO.StatusCode, res.StatusCode);
        }
    }
}
