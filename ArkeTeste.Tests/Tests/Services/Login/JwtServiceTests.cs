using ArkeTest.Models;
using ArkeTest.Services.Login;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Moq;

namespace ArkeTeste.Tests.Tests.Services.Login
{
    public class JwtServiceTests
    {
        [Fact]
        public void JwtService_GenerateJwtToken_Succes()
        {
            // Arrange
            Mock<IUserStore<ApplicationUser>> store = new();
#pragma warning disable CS8625 // Cannot convert null literal to non-nullable reference type.
            Mock<UserManager<ApplicationUser>> mockUserManager = new(store.Object, null, null, null, null, null, null, null, null);
#pragma warning restore CS8625 // Cannot convert null literal to non-nullable reference type.
            Mock<IHttpContextAccessor> mockIHttpContextAccessor = new();
            Mock<ILogger<JwtService>> mockILogger = new();
            Mock<HttpContext> mockHttpContext = new();
            Mock<HttpResponse> mockResponse = new();
            Mock<IResponseCookies> mockCookieCollection = new();
            JwtService jwtService = new(mockUserManager.Object, mockIHttpContextAccessor.Object, mockILogger.Object);

            ApplicationUser applicationUser = new() { Id = new Guid().ToString(), UserName = "test@test.com" };

            mockResponse.SetupGet(r => r.Cookies).Returns(mockCookieCollection.Object);

            // Setup the HttpContext to return the mock response
            mockHttpContext.SetupGet(c => c.Response).Returns(mockResponse.Object);

            // Setup the HttpContextAccessor to return the mock HttpContext
            mockIHttpContextAccessor.SetupGet(a => a.HttpContext).Returns(mockHttpContext.Object);

            jwtService.GenerateJwtToken(applicationUser);

            mockCookieCollection.Verify(c => c.Append("jwt", It.IsAny<string>(), It.IsAny<CookieOptions>()), Times.Once);
        }


        [Fact]
        public async Task JwtService_GenerateRefreshToken_Succes()
        {
            // Arrange
            Mock<IUserStore<ApplicationUser>> store = new();
#pragma warning disable CS8625 // Cannot convert null literal to non-nullable reference type.
            Mock<UserManager<ApplicationUser>> mockUserManager = new(store.Object, null, null, null, null, null, null, null, null);
#pragma warning restore CS8625 // Cannot convert null literal to non-nullable reference type.
            Mock<IHttpContextAccessor> mockIHttpContextAccessor = new();
            Mock<ILogger<JwtService>> mockILogger = new();
            Mock<HttpContext> mockHttpContext = new();
            Mock<HttpResponse> mockResponse = new();
            Mock<IResponseCookies> mockCookieCollection = new();
            JwtService jwtService = new(mockUserManager.Object, mockIHttpContextAccessor.Object, mockILogger.Object);

            ApplicationUser applicationUser = new() { Id = new Guid().ToString(), UserName = "test@test.com" };

            mockResponse.SetupGet(r => r.Cookies).Returns(mockCookieCollection.Object);

            // Setup the HttpContext to return the mock response
            mockHttpContext.SetupGet(c => c.Response).Returns(mockResponse.Object);

            // Setup the HttpContextAccessor to return the mock HttpContext
            mockIHttpContextAccessor.SetupGet(a => a.HttpContext).Returns(mockHttpContext.Object);

            mockUserManager.Setup(x => x.UpdateAsync(It.IsAny<ApplicationUser>()))
                       .ReturnsAsync(IdentityResult.Success);

            await jwtService.GenerateRefreshToken(applicationUser);

            mockCookieCollection.Verify(c => c.Append("refreshToken", It.IsAny<string>(), It.IsAny<CookieOptions>()), Times.Once);
        }

        [Fact]
        public async Task JwtService_GenerateRefreshToken_Failed()
        {
            // Arrange
            Mock<IUserStore<ApplicationUser>> store = new();
#pragma warning disable CS8625 // Cannot convert null literal to non-nullable reference type.
            Mock<UserManager<ApplicationUser>> mockUserManager = new(store.Object, null, null, null, null, null, null, null, null);
#pragma warning restore CS8625 // Cannot convert null literal to non-nullable reference type.
            Mock<IHttpContextAccessor> mockIHttpContextAccessor = new();
            Mock<ILogger<JwtService>> mockILogger = new();
            Mock<HttpContext> mockHttpContext = new();
            Mock<HttpResponse> mockResponse = new();
            Mock<IResponseCookies> mockCookieCollection = new();
            JwtService jwtService = new(mockUserManager.Object, mockIHttpContextAccessor.Object, mockILogger.Object);

            ApplicationUser applicationUser = new() { Id = new Guid().ToString(), UserName = "test@test.com" };

            mockResponse.SetupGet(r => r.Cookies).Returns(mockCookieCollection.Object);

            // Setup the HttpContext to return the mock response
            mockHttpContext.SetupGet(c => c.Response).Returns(mockResponse.Object);

            // Setup the HttpContextAccessor to return the mock HttpContext
            mockIHttpContextAccessor.SetupGet(a => a.HttpContext).Returns(mockHttpContext.Object);

            mockUserManager.Setup(x => x.UpdateAsync(It.IsAny<ApplicationUser>()))
                       .ReturnsAsync(IdentityResult.Failed(new IdentityError { Description = "Error updating user" }));

            await jwtService.GenerateRefreshToken(applicationUser);

            mockCookieCollection.Verify(c => c.Append("refreshToken", It.IsAny<string>(), It.IsAny<CookieOptions>()), Times.Never);
            ;
        }

    }
}
