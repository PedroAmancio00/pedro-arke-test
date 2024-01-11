using ArkeTest.DTO;
using ArkeTest.Services.Jwt.IJwt;
using ArkeTest.Services.Login;
using Microsoft.Extensions.Logging;
using Moq;

namespace ArkeTeste.Tests.Tests.Services.Login
{
    public class LogoutServiceTest
    {
        private readonly LogoutService _logoutService;
        private readonly Mock<IJwtService> _mockIJwtService;
        private readonly Mock<ILogger<LogoutService>> _mockILogger;


        public LogoutServiceTest()
        {
            _mockILogger = new Mock<ILogger<LogoutService>>();
            _mockIJwtService = new Mock<IJwtService>();

            _logoutService = new(_mockIJwtService.Object, _mockILogger.Object);
        }

        [Fact]
        public void LogoutService_Logout_Success()
        {
            // Mocking functions

            _mockIJwtService.Setup(s => s.RemoveTokens()).Verifiable();

            // Getting result
            ReturnDTO res = _logoutService.Logout();

            ReturnDTO returnDTO = new()
            {
                Message = "Logout successfully",
                StatusCode = System.Net.HttpStatusCode.OK
            };

            // Asserting
            Assert.NotNull(res);
            Assert.Equal(returnDTO.Message, res.Message);
            Assert.Equal(returnDTO.StatusCode, res.StatusCode);
        }
    }
}
