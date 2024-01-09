using ArkeTest.Data;
using ArkeTest.Models;
using ArkeTest.Services.Login;
using ArkeTest.Services.Login.ILogin;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArkeTeste.Tests.Tests.Services
{
    public class RefreshServiceTest
    {
        [Fact]
        public async Task RefreshService_Refresh_Success()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<MyDbContext>()
                .UseInMemoryDatabase(databaseName: "RefreshService_Refresh_Success") // Make sure to use a unique name for each test
                .Options;
            // Create an instance of MyDbContext with the in-memory options
            var mockMyDbContext = new MyDbContext(options);

            var testData = new List<ApplicationUser>
            {
                new ApplicationUser { RefreshToken = "token", RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(7) }
            };

            // Adicione os dados de teste ao contexto
            mockMyDbContext.ApplicationUsers.AddRange(testData);
            mockMyDbContext.SaveChanges();

            Mock<IHttpContextAccessor> mockIHttpContextAccessor = new();
            Mock<ILogger<RefreshService>> mockILogger = new();
            Mock<IJwtService> mockIJwtService = new();
            Mock<IResponseCookies> mockCookieCollection = new();
            RefreshService refreshService = new(mockMyDbContext, mockIJwtService.Object, mockILogger.Object);

            mockIJwtService.Setup(s => s.GetRefreshToken()).Verifiable();

            mockIJwtService.Setup(s => s.GenerateJwtToken(It.IsAny<ApplicationUser>())).Verifiable();

            await refreshService.Refresh();

            mockCookieCollection.Verify(c => c.Append("jwt", It.IsAny<string>(), It.IsAny<CookieOptions>()), Times.Once);
        }
    }
}
