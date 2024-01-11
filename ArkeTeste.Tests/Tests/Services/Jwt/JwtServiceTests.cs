using ArkeTest.Models;
using ArkeTest.Services.Factory.IFactory;
using ArkeTest.Services.Jwt;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Moq;
using System.Security.Claims;
using System.Text;

namespace ArkeTeste.Tests.Tests.Services.Jwt
{
    public class JwtServiceTests
    {
        private readonly JwtService _jwtService;
        private readonly Mock<IUserStore<ApplicationUser>> _store;
        private readonly Mock<UserManager<ApplicationUser>> _mockUserManager;
        private readonly Mock<IHttpContextAccessor> _mockIHttpContextAccessor;
        private readonly Mock<ILogger<JwtService>> _mockILogger;
        private readonly Mock<HttpContext> _mockHttpContext;
        private readonly Mock<HttpResponse> _mockResponse;
        private readonly Mock<IRequestCookieCollection> _mockCookieCollection;
        private readonly Mock<IConfiguration> _mockIConfiguration;
        private readonly Mock<ITokenValidationParametersFactory> _mockITokenValidationParametersFactory;
        private readonly Mock<ITokenHandlerWrapper> _mockITokenHandlerWrapper;
        private readonly Mock<HttpRequest> _mockRequest;
        private readonly Mock<IResponseCookies> _mockIResponseCookies;

        public JwtServiceTests()
        {
            _store = new Mock<IUserStore<ApplicationUser>>();
#pragma warning disable CS8625 // Cannot convert null literal to non-nullable reference type.
            _mockUserManager = new Mock<UserManager<ApplicationUser>>(_store.Object, null, null, null, null, null, null, null, null);
#pragma warning restore CS8625 // Cannot convert null literal to non-nullable reference type.
            _mockIHttpContextAccessor = new Mock<IHttpContextAccessor>();
            _mockILogger = new Mock<ILogger<JwtService>>();
            _mockHttpContext = new Mock<HttpContext>();
            _mockResponse = new Mock<HttpResponse>();
            _mockCookieCollection = new Mock<IRequestCookieCollection>();
            _mockIConfiguration = new Mock<IConfiguration>();
            _mockITokenValidationParametersFactory = new Mock<ITokenValidationParametersFactory>();
            _mockITokenHandlerWrapper = new Mock<ITokenHandlerWrapper>();
            _mockRequest = new Mock<HttpRequest>();
            _mockIResponseCookies = new Mock<IResponseCookies>();

            _jwtService = new(_mockUserManager.Object,
                                _mockIHttpContextAccessor.Object,
                                _mockILogger.Object,
                                _mockIConfiguration.Object,
                                _mockITokenValidationParametersFactory.Object,
                                _mockITokenHandlerWrapper.Object);
        }

        [Fact]
        public void JwtService_GenerateJwtToken_Success()
        {
            ApplicationUser applicationUser = new() { Id = new Guid().ToString(), UserName = "test@test.com" };

            // Mocking context
            _mockResponse.SetupGet(x => x.Cookies).Returns(_mockIResponseCookies.Object);

            _mockHttpContext.SetupGet(x => x.Response).Returns(_mockResponse.Object);

            _mockIHttpContextAccessor.SetupGet(x => x.HttpContext).Returns(_mockHttpContext.Object);

            // Mock Key
            string key = "e2e253a877ce8e0c23ca6d0ed0be12a39cc7b4e553302ddc226c6924ea66c0d5";

            _mockIConfiguration.Setup(x => x["jwtKey"]).Returns(key);

            // Getting result
            _jwtService.GenerateJwtToken(applicationUser);

            // Asserting
            _mockIResponseCookies.Verify(x => x.Append("jwt", It.IsAny<string>(), It.IsAny<CookieOptions>()), Times.Once);
        }


        [Fact]
        public async Task JwtService_GenerateRefreshToken_Success()
        {
            ApplicationUser applicationUser = new() { Id = new Guid().ToString(), UserName = "test@test.com" };

            // Mocking context
            _mockResponse.SetupGet(x => x.Cookies).Returns(_mockIResponseCookies.Object);

            _mockHttpContext.SetupGet(x => x.Response).Returns(_mockResponse.Object);

            _mockIHttpContextAccessor.SetupGet(x => x.HttpContext).Returns(_mockHttpContext.Object);

            //Mocking functions
            _mockUserManager.Setup(x => x.UpdateAsync(It.IsAny<ApplicationUser>()))
                       .ReturnsAsync(IdentityResult.Success);

            await _jwtService.GenerateRefreshToken(applicationUser);

            _mockIResponseCookies.Verify(x => x.Append("refreshToken", It.IsAny<string>(), It.IsAny<CookieOptions>()), Times.Once);
        }

        [Fact]
        public async Task JwtService_GenerateRefreshToken_Failed()
        {
            ApplicationUser applicationUser = new() { Id = new Guid().ToString(), UserName = "test@test.com" };

            // Mocking context
            _mockResponse.SetupGet(x => x.Cookies).Returns(_mockIResponseCookies.Object);


            _mockHttpContext.SetupGet(x => x.Response).Returns(_mockResponse.Object);


            _mockIHttpContextAccessor.SetupGet(x => x.HttpContext).Returns(_mockHttpContext.Object);

            //Mocking functions to fail
            _mockUserManager.Setup(x => x.UpdateAsync(It.IsAny<ApplicationUser>()))
                       .ReturnsAsync(IdentityResult.Failed(new IdentityError { Description = "Error updating user" }));

            await _jwtService.GenerateRefreshToken(applicationUser);

            _mockIResponseCookies.Verify(x => x.Append("refreshToken", It.IsAny<string>(), It.IsAny<CookieOptions>()), Times.Never);
        }

        [Fact]
        public void JwtService_RemoveTokens_Success()
        {
            ApplicationUser applicationUser = new() { Id = new Guid().ToString(), UserName = "test@test.com" };

            // Mocking functions
            _mockIHttpContextAccessor.Setup(x => x.HttpContext).Returns(_mockHttpContext.Object);
            _mockHttpContext.Setup(x => x.Response).Returns(_mockResponse.Object);
            _mockResponse.Setup(x => x.Cookies).Returns(_mockIResponseCookies.Object);

            // Getting result
            _jwtService.RemoveTokens();

            // Asserting
            _mockResponse.Verify(x => x.Cookies.Delete("jwt"), Times.Once);
            _mockResponse.Verify(x => x.Cookies.Delete("refreshToken"), Times.Once);
        }


        [Fact]
        public void JwtService_GetRefreshToken_Success()
        {
            // Mocking context
            _mockIHttpContextAccessor.Setup(x => x.HttpContext).Returns(_mockHttpContext.Object);
            _mockHttpContext.Setup(x => x.Request).Returns(_mockRequest.Object);
            _mockRequest.Setup(x => x.Cookies).Returns(_mockCookieCollection.Object);


            // Mocking functions
            string? token = "refresh";
            _mockCookieCollection.Setup(x => x.TryGetValue("refreshToken", out token)).Returns(true);

            // Getting result
            string? res = _jwtService.GetRefreshToken();

            Assert.NotNull(res);
            _mockCookieCollection.Verify(x => x.TryGetValue("refreshToken", out token), Times.Once);
        }

        [Fact]
        public void JwtService_GetRefreshToken_Failed()
        {
            // Mocking context
            _mockIHttpContextAccessor.Setup(x => x.HttpContext).Returns(_mockHttpContext.Object);
            _mockHttpContext.Setup(x => x.Request).Returns(_mockRequest.Object);
            _mockRequest.Setup(x => x.Cookies).Returns(_mockCookieCollection.Object);


            // Mocking functions
            string? token = null;
            _mockCookieCollection.Setup(x => x.TryGetValue("refreshToken", out token)).Returns(true);

            // Getting result
            string? res = _jwtService.GetRefreshToken();

            Assert.Null(res);
            _mockCookieCollection.Verify(x => x.TryGetValue("refreshToken", out token), Times.Once);
        }

        [Fact]
        public void JwtService_GetAndDecodeJwtToken_Success()
        {
            // Mocking context
            _mockIHttpContextAccessor.Setup(x => x.HttpContext).Returns(_mockHttpContext.Object);
            _mockHttpContext.Setup(x => x.Request).Returns(_mockRequest.Object);
            _mockRequest.Setup(x => x.Cookies).Returns(_mockCookieCollection.Object);


            // Mocking functions
            string? token = "jwt";
            _mockCookieCollection.Setup(x => x.TryGetValue("jwt", out token)).Returns(true);

            // Mock key
            string key = "e2e253a877ce8e0c23ca6d0ed0be12a39cc7b4e553302ddc226c6924ea66c0d5";

            TokenValidationParameters tokenValidationParameters = new()
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key)),
                ValidateIssuer = false,
                ValidateAudience = false,
                ValidateLifetime = true,
                ClockSkew = TimeSpan.Zero
            };


            _mockITokenValidationParametersFactory.Setup(x => x.Create(It.IsAny<string>())).Returns(new TokenValidationParameters());

            List<Claim> claims = new()
            {
                new Claim("jti", "mockGuid")
            };
            ClaimsIdentity identity = new(claims);
            ClaimsPrincipal expectedPrincipal = new(identity);

            _mockITokenHandlerWrapper.Setup(t => t.ValidateToken(token, It.IsAny<TokenValidationParameters>(), out It.Ref<SecurityToken>.IsAny))
                               .Returns(expectedPrincipal);

            // Getting result
            string? res = _jwtService.GetAndDecodeJwtToken();

            Assert.NotNull(res);
            Assert.Equal("mockGuid", res);
            _mockITokenValidationParametersFactory.Verify(x => x.Create(It.IsAny<string>()), Times.Once);
            _mockITokenHandlerWrapper.Verify(x => x.ValidateToken(token, It.IsAny<TokenValidationParameters>(), out It.Ref<SecurityToken>.IsAny), Times.Once);
        }

        [Fact]
        public void JwtService_GetAndDecodeJwtToken_Jwt_Failed()
        {
            // Mocking context
            _mockIHttpContextAccessor.Setup(x => x.HttpContext).Returns(_mockHttpContext.Object);
            _mockHttpContext.Setup(x => x.Request).Returns(_mockRequest.Object);
            _mockRequest.Setup(x => x.Cookies).Returns(_mockCookieCollection.Object);


            // Mocking functions
            string? token = "jwt";
            _mockCookieCollection.Setup(x => x.TryGetValue("jwt", out token)).Returns(true);

            // Mock key
            string key = "e2e253a877ce8e0c23ca6d0ed0be12a39cc7b4e553302ddc226c6924ea66c0d5";

            TokenValidationParameters tokenValidationParameters = new()
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key)),
                ValidateIssuer = false,
                ValidateAudience = false,
                ValidateLifetime = true,
                ClockSkew = TimeSpan.Zero
            };


            _mockITokenValidationParametersFactory.Setup(x => x.Create(It.IsAny<string>())).Returns(new TokenValidationParameters());

            List<Claim> claims = new()
            {

            };
            ClaimsIdentity identity = new(claims);
            ClaimsPrincipal expectedPrincipal = new(identity);

            _mockITokenHandlerWrapper.Setup(t => t.ValidateToken(token, It.IsAny<TokenValidationParameters>(), out It.Ref<SecurityToken>.IsAny))
                               .Returns(expectedPrincipal);

            // Getting result
            string? res = _jwtService.GetAndDecodeJwtToken();

            Assert.Null(res);
            _mockITokenValidationParametersFactory.Verify(x => x.Create(It.IsAny<string>()), Times.Once);
            _mockITokenHandlerWrapper.Verify(x => x.ValidateToken(token, It.IsAny<TokenValidationParameters>(), out It.Ref<SecurityToken>.IsAny), Times.Once);
        }

    }
}
