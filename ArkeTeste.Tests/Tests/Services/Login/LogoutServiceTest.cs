using ArkeTest.DTO;
using ArkeTest.Services.Jwt.IJwt;
using ArkeTest.Services.Login;
using Microsoft.Extensions.Logging;
using Moq;

namespace ArkeTeste.Tests.Tests.Services.Login
{
    public class LogoutServiceTest
    {
        [Fact]
        public void LogoutService_Logout_Success()
        {
            // Mocking service
            Mock<ILogger<LogoutService>> mockILogger = new();
            Mock<IJwtService> mockIJwtService = new();
            LogoutService logoutService = new(mockIJwtService.Object, mockILogger.Object);

            // Mocking functions

            mockIJwtService.Setup(s => s.RemoveTokens()).Verifiable();

            // Getting result
            ReturnDTO res = logoutService.Logout();

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
