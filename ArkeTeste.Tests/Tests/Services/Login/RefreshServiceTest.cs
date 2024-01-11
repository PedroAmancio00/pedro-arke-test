using ArkeTest.Data;
using ArkeTest.DTO;
using ArkeTest.Models;
using ArkeTest.Services.Jwt.IJwt;
using ArkeTest.Services.Login;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;

namespace ArkeTeste.Tests.Tests.Services
{
    public class RefreshServiceTest
    {
        private readonly RefreshService _refreshService;
        private readonly MyDbContext _mockMyDbContext;
        private readonly Mock<IJwtService> _mockIJwtService;
        private readonly Mock<ILogger<RefreshService>> _mockILogger;


        public RefreshServiceTest()
        {
            DbContextOptions<MyDbContext> options = new DbContextOptionsBuilder<MyDbContext>()
                   .UseInMemoryDatabase(databaseName: "RefreshService_Refresh")
                   .Options;
            _mockMyDbContext = new(options);
            _mockILogger = new Mock<ILogger<RefreshService>>();
            _mockIJwtService = new Mock<IJwtService>();

            _refreshService = new(_mockMyDbContext, _mockIJwtService.Object, _mockILogger.Object);
        }

        [Fact]
        public async Task RefreshService_Refresh_Success()
        {
            // Adding data on database
            List<ApplicationUser> testData = new()
            {
                new ApplicationUser { RefreshToken = "token", RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(7) }
            };

            _mockMyDbContext.ApplicationUsers.AddRange(testData);
            _mockMyDbContext.SaveChanges();

            // Mocking functions
            _mockIJwtService.Setup(s => s.GetRefreshToken()).Returns("token");

            _mockIJwtService.Setup(s => s.GenerateJwtToken(It.IsAny<ApplicationUser>())).Verifiable();

            // Getting result
            ReturnDTO res = await _refreshService.Refresh();

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
            _mockMyDbContext.Database.EnsureDeleted();

            // Nullifying token for failure
            string? token = null;

            // Mocking functions
            _mockIJwtService.Setup(s => s.GetRefreshToken()).Returns(token);

            // Getting result
            ReturnDTO res = await _refreshService.Refresh();

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
            _mockMyDbContext.Database.EnsureDeleted();

            string? token = "token";

            // Mocking functions
            _mockIJwtService.Setup(s => s.GetRefreshToken()).Returns(token);

            // Getting result
            ReturnDTO res = await _refreshService.Refresh();

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
