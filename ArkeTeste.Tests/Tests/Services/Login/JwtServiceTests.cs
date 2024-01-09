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
        public void JwtService_GenerateJwtToken_Success()
        {
            // Mocking services
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

            // Mocking context
            mockResponse.SetupGet(x => x.Cookies).Returns(mockCookieCollection.Object);

            mockHttpContext.SetupGet(x => x.Response).Returns(mockResponse.Object);

            mockIHttpContextAccessor.SetupGet(x => x.HttpContext).Returns(mockHttpContext.Object);

            // Getting result
            jwtService.GenerateJwtToken(applicationUser);

            // Asserting
            mockCookieCollection.Verify(x => x.Append("jwt", It.IsAny<string>(), It.IsAny<CookieOptions>()), Times.Once);
        }


        [Fact]
        public async Task JwtService_GenerateRefreshToken_Success()
        {
            // Mocking services
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

            // Mocking context
            mockResponse.SetupGet(x => x.Cookies).Returns(mockCookieCollection.Object);

            mockHttpContext.SetupGet(x => x.Response).Returns(mockResponse.Object);

            mockIHttpContextAccessor.SetupGet(x => x.HttpContext).Returns(mockHttpContext.Object);

            //Mocking functions
            mockUserManager.Setup(x => x.UpdateAsync(It.IsAny<ApplicationUser>()))
                       .ReturnsAsync(IdentityResult.Success);

            await jwtService.GenerateRefreshToken(applicationUser);

            mockCookieCollection.Verify(x => x.Append("refreshToken", It.IsAny<string>(), It.IsAny<CookieOptions>()), Times.Once);
        }

        [Fact]
        public async Task JwtService_GenerateRefreshToken_Failed()
        {
            // Mocking services
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

            // Mocking context
            mockResponse.SetupGet(x => x.Cookies).Returns(mockCookieCollection.Object);


            mockHttpContext.SetupGet(x => x.Response).Returns(mockResponse.Object);


            mockIHttpContextAccessor.SetupGet(x => x.HttpContext).Returns(mockHttpContext.Object);

            //Mocking functions to fail
            mockUserManager.Setup(x => x.UpdateAsync(It.IsAny<ApplicationUser>()))
                       .ReturnsAsync(IdentityResult.Failed(new IdentityError { Description = "Error updating user" }));

            await jwtService.GenerateRefreshToken(applicationUser);

            mockCookieCollection.Verify(x => x.Append("refreshToken", It.IsAny<string>(), It.IsAny<CookieOptions>()), Times.Never);
        }

        [Fact]
        public void JwtService_RemoveTokens_Success()
        {
            // Mocking services
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

            Mock<HttpResponse> responseMock = new();
            // Mocking functions
            mockIHttpContextAccessor.Setup(x => x.HttpContext).Returns(mockHttpContext.Object);
            mockHttpContext.Setup(x => x.Response).Returns(responseMock.Object);
            responseMock.Setup(x => x.Cookies).Returns(mockCookieCollection.Object);

            // Getting result
            jwtService.RemoveTokens();

            // Asserting
            responseMock.Verify(x => x.Cookies.Delete("jwt"), Times.Once);
            responseMock.Verify(x => x.Cookies.Delete("refreshToken"), Times.Once);
        }


        [Fact]
        public void JwtService_GetRefreshToken_Success()
        {
            // Mocking services
            Mock<IUserStore<ApplicationUser>> store = new();
#pragma warning disable CS8625 // Cannot convert null literal to non-nullable reference type.
            Mock<UserManager<ApplicationUser>> mockUserManager = new(store.Object, null, null, null, null, null, null, null, null);
#pragma warning restore CS8625 // Cannot convert null literal to non-nullable reference type.            
            Mock<ILogger<JwtService>> mockILogger = new();
            Mock<IHttpContextAccessor> mockIHttpContextAccessor = new();
            Mock<HttpContext> mockHttpContext = new();
            Mock<HttpRequest> mockRequest = new();
            Mock<IRequestCookieCollection> mockCookieCollection = new();
            JwtService jwtService = new(mockUserManager.Object, mockIHttpContextAccessor.Object, mockILogger.Object);

            // Mocking context
            mockIHttpContextAccessor.Setup(x => x.HttpContext).Returns(mockHttpContext.Object);
            mockHttpContext.Setup(x => x.Request).Returns(mockRequest.Object);
            mockRequest.Setup(x => x.Cookies).Returns(mockCookieCollection.Object);


            // Mocking functions
            string? token = "refresh";
            mockCookieCollection.Setup(x => x.TryGetValue("refreshToken", out token)).Returns(true);

            // Getting result
            string? res = jwtService.GetRefreshToken();

            Assert.NotNull(res);
            mockCookieCollection.Verify(x => x.TryGetValue("refreshToken", out token), Times.Once);
        }

        [Fact]
        public void JwtService_GetRefreshToken_Failed()
        {
            // Mocking services
            Mock<IUserStore<ApplicationUser>> store = new();
#pragma warning disable CS8625 // Cannot convert null literal to non-nullable reference type.
            Mock<UserManager<ApplicationUser>> mockUserManager = new(store.Object, null, null, null, null, null, null, null, null);
#pragma warning restore CS8625 // Cannot convert null literal to non-nullable reference type.            
            Mock<ILogger<JwtService>> mockILogger = new();
            Mock<IHttpContextAccessor> mockIHttpContextAccessor = new();
            Mock<HttpContext> mockHttpContext = new();
            Mock<HttpRequest> mockRequest = new();
            Mock<IRequestCookieCollection> mockCookieCollection = new();
            JwtService jwtService = new(mockUserManager.Object, mockIHttpContextAccessor.Object, mockILogger.Object);

            // Mocking context
            mockIHttpContextAccessor.Setup(x => x.HttpContext).Returns(mockHttpContext.Object);
            mockHttpContext.Setup(x => x.Request).Returns(mockRequest.Object);
            mockRequest.Setup(x => x.Cookies).Returns(mockCookieCollection.Object);


            // Mocking functions
            string? token = null;
            mockCookieCollection.Setup(x => x.TryGetValue("refreshToken", out token)).Returns(true);

            // Getting result
            string? res = jwtService.GetRefreshToken();

            Assert.Null(res);
            mockCookieCollection.Verify(x => x.TryGetValue("refreshToken", out token), Times.Once);
        }

    }
}
