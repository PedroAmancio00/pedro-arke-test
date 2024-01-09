using ArkeTest.Controllers;
using ArkeTest.DTO;
using ArkeTest.DTO.Login;
using ArkeTest.Services.Login.ILogin;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System.Net;

namespace ArkeTeste.Tests.Tests.Controllers.Login
{
    public class LoginControllerTests
    {
        [Fact]
        public async Task CreateLogin_Sucess()
        {
            Mock<ICreateLoginService> mockCreateLoginService = new();
            Mock<IAccessAccountService> accessAccountService = new();
            Mock<IRefreshService> refreshService = new();
            Mock<ILogoutService> logoutService = new();
            LoginController loginController = new(mockCreateLoginService.Object,
                                                  accessAccountService.Object,
                                                  refreshService.Object,
                                                  logoutService.Object);

            ReturnDTO returnDTO = new()
            {
                Message = "Login created",
                StatusCode = HttpStatusCode.Created
            };

            mockCreateLoginService.Setup(x => x.CreateLogin(It.IsAny<CreateLoginDTO>()))
                .ReturnsAsync(new ReturnDTO() { Message = "Login created", StatusCode = HttpStatusCode.Created });

            IActionResult result = await loginController.CreateLogin(new CreateLoginDTO());

            Assert.NotNull(result);
            if (result is ObjectResult objectResult)
            {
                string? message = objectResult.Value?.ToString();
                HttpStatusCode? statusCode = (HttpStatusCode?)objectResult.StatusCode;
                Assert.Equal(returnDTO.Message, message);
                Assert.Equal(returnDTO.StatusCode, statusCode);
            }
        }

        [Fact]
        public async Task CreateLogin_Conflict()
        {
            Mock<ICreateLoginService> mockCreateLoginService = new();
            Mock<IAccessAccountService> accessAccountService = new();
            Mock<IRefreshService> refreshService = new();
            Mock<ILogoutService> logoutService = new();
            LoginController loginController = new(mockCreateLoginService.Object,
                                                  accessAccountService.Object,
                                                  refreshService.Object,
                                                  logoutService.Object);

            mockCreateLoginService.Setup(x => x.CreateLogin(It.IsAny<CreateLoginDTO>()))
                .ReturnsAsync(new ReturnDTO() { Message = "Error", StatusCode = HttpStatusCode.Conflict });

            IActionResult result = await loginController.CreateLogin(new CreateLoginDTO());

            Assert.NotNull(result);
            if (result is ObjectResult objectResult)
            {
                string? message = objectResult.Value?.ToString();
                HttpStatusCode? statusCode = (HttpStatusCode?)objectResult.StatusCode;
                Assert.NotNull(message);
                Assert.Equal(HttpStatusCode.Conflict, statusCode);
            }
        }

        [Fact]
        public async Task CreateLogin_InterServerError()
        {
            Mock<ICreateLoginService> mockCreateLoginService = new();
            Mock<IAccessAccountService> accessAccountService = new();
            Mock<IRefreshService> refreshService = new();
            Mock<ILogoutService> logoutService = new();
            LoginController loginController = new(mockCreateLoginService.Object,
                                                  accessAccountService.Object,
                                                  refreshService.Object,
                                                  logoutService.Object);

            mockCreateLoginService.Setup(x => x.CreateLogin(It.IsAny<CreateLoginDTO>()))
                .ReturnsAsync(new ReturnDTO() { Message = "Error", StatusCode = HttpStatusCode.InternalServerError });

            ReturnDTO returnDTO = new()
            {
                Message = "Error",
                StatusCode = HttpStatusCode.InternalServerError
            };

            IActionResult result = await loginController.CreateLogin(new CreateLoginDTO());

            Assert.NotNull(result);
            if (result is ObjectResult objectResult)
            {
                string? message = objectResult.Value?.ToString();
                HttpStatusCode? statusCode = (HttpStatusCode?)objectResult.StatusCode;
                Assert.Equal(returnDTO.StatusCode, statusCode);
                Assert.Equal(returnDTO.StatusCode, statusCode);
            }
        }

        [Fact]
        public async Task AccessAccount_Sucess()
        {
            Mock<ICreateLoginService> mockCreateLoginService = new();
            Mock<IAccessAccountService> accessAccountService = new();
            Mock<IRefreshService> refreshService = new();
            Mock<ILogoutService> logoutService = new();
            LoginController loginController = new(mockCreateLoginService.Object,
                                                  accessAccountService.Object,
                                                  refreshService.Object,
                                                  logoutService.Object);

            ReturnDTO returnDTO = new()
            {
                Message = "Successful Login",
                StatusCode = HttpStatusCode.OK
            };

            accessAccountService.Setup(x => x.AccessAccount(It.IsAny<AccessAccountDTO>()))
                .ReturnsAsync(new ReturnDTO() { Message = "Successful Login", StatusCode = HttpStatusCode.OK });

            IActionResult result = await loginController.AccessAccount(new AccessAccountDTO());


            Assert.NotNull(result);
            if (result is ObjectResult objectResult)
            {
                string? message = objectResult.Value?.ToString();
                HttpStatusCode? statusCode = (HttpStatusCode?)objectResult.StatusCode;
                Assert.Equal(returnDTO.Message, message);
                Assert.Equal(returnDTO.StatusCode, statusCode);
            }
        }

        [Fact]
        public async Task AccessAccount_NotFound()
        {
            Mock<ICreateLoginService> mockCreateLoginService = new();
            Mock<IAccessAccountService> accessAccountService = new();
            Mock<IRefreshService> refreshService = new();
            Mock<ILogoutService> logoutService = new();
            LoginController loginController = new(mockCreateLoginService.Object,
                                                  accessAccountService.Object,
                                                  refreshService.Object,
                                                  logoutService.Object);

            accessAccountService.Setup(x => x.AccessAccount(It.IsAny<AccessAccountDTO>()))
                .ReturnsAsync(new ReturnDTO() { Message = "Email or Password is wrong", StatusCode = HttpStatusCode.NotFound });

            IActionResult result = await loginController.AccessAccount(new AccessAccountDTO());

            ReturnDTO returnDTO = new()
            {
                Message = "Email or Password is wrong",
                StatusCode = HttpStatusCode.NotFound
            };

            Assert.NotNull(result);
            if (result is ObjectResult objectResult)
            {
                string? message = objectResult.Value?.ToString();
                HttpStatusCode? statusCode = (HttpStatusCode?)objectResult.StatusCode;
                Assert.Equal(returnDTO.Message, message);
                Assert.Equal(returnDTO.StatusCode, statusCode);
            }
        }

        [Fact]
        public async Task AccessAccount_InterServerError()
        {
            Mock<ICreateLoginService> mockCreateLoginService = new();
            Mock<IAccessAccountService> accessAccountService = new();
            Mock<IRefreshService> refreshService = new();
            Mock<ILogoutService> logoutService = new();
            LoginController loginController = new(mockCreateLoginService.Object,
                                                  accessAccountService.Object,
                                                  refreshService.Object,
                                                  logoutService.Object);

            accessAccountService.Setup(x => x.AccessAccount(It.IsAny<AccessAccountDTO>()))
                .ReturnsAsync(new ReturnDTO() { Message = "Error", StatusCode = HttpStatusCode.InternalServerError });

            IActionResult result = await loginController.AccessAccount(new AccessAccountDTO());

            ReturnDTO returnDTO = new()
            {
                Message = "Error",
                StatusCode = HttpStatusCode.InternalServerError
            };

            Assert.NotNull(result);
            if (result is ObjectResult objectResult)
            {
                string? message = objectResult.Value?.ToString();
                HttpStatusCode? statusCode = (HttpStatusCode?)objectResult.StatusCode;
                Assert.Equal(returnDTO.Message, message);
                Assert.Equal(returnDTO.StatusCode, statusCode);
            }
        }

        [Fact]
        public async Task Refresh_Sucess()
        {
            Mock<ICreateLoginService> mockCreateLoginService = new();
            Mock<IAccessAccountService> accessAccountService = new();
            Mock<IRefreshService> refreshService = new();
            Mock<ILogoutService> logoutService = new();
            LoginController loginController = new(mockCreateLoginService.Object,
                                                  accessAccountService.Object,
                                                  refreshService.Object,
                                                  logoutService.Object);


            refreshService.Setup(x => x.Refresh())
                .ReturnsAsync(new ReturnDTO() { Message = "Refreshed", StatusCode = HttpStatusCode.OK });

            ReturnDTO returnDTO = new()
            {
                Message = "Refreshed",
                StatusCode = HttpStatusCode.OK
            };

            IActionResult result = await loginController.Refresh();


            Assert.NotNull(result);
            if (result is ObjectResult objectResult)
            {
                string? message = objectResult.Value?.ToString();
                HttpStatusCode? statusCode = (HttpStatusCode?)objectResult.StatusCode;
                Assert.Equal(returnDTO.Message, message);
                Assert.Equal(returnDTO.StatusCode, statusCode);
            }
        }

        [Fact]
        public async Task Refresh_NotFound()
        {
            Mock<ICreateLoginService> mockCreateLoginService = new();
            Mock<IAccessAccountService> accessAccountService = new();
            Mock<IRefreshService> refreshService = new();
            Mock<ILogoutService> logoutService = new();
            LoginController loginController = new(mockCreateLoginService.Object,
                                                  accessAccountService.Object,
                                                  refreshService.Object,
                                                  logoutService.Object);

            refreshService.Setup(x => x.Refresh())
                .ReturnsAsync(new ReturnDTO() { Message = "Refresh failed", StatusCode = HttpStatusCode.NotFound });

            IActionResult result = await loginController.Refresh();

            ReturnDTO returnDTO = new()
            {
                Message = "Refresh failed",
                StatusCode = HttpStatusCode.NotFound
            };

            Assert.NotNull(result);
            if (result is ObjectResult objectResult)
            {
                string? message = objectResult.Value?.ToString();
                HttpStatusCode? statusCode = (HttpStatusCode?)objectResult.StatusCode;
                Assert.Equal(returnDTO.Message, message);
                Assert.Equal(returnDTO.StatusCode, statusCode);
            }
        }

        [Fact]
        public async Task Refresh_InterServerError()
        {
            Mock<ICreateLoginService> mockCreateLoginService = new();
            Mock<IAccessAccountService> accessAccountService = new();
            Mock<IRefreshService> refreshService = new();
            Mock<ILogoutService> logoutService = new();
            LoginController loginController = new(mockCreateLoginService.Object,
                                                  accessAccountService.Object,
                                                  refreshService.Object,
                                                  logoutService.Object);

            refreshService.Setup(x => x.Refresh())
                .ReturnsAsync(new ReturnDTO() { Message = "Error", StatusCode = HttpStatusCode.InternalServerError });

            IActionResult result = await loginController.Refresh();

            ReturnDTO returnDTO = new()
            {
                Message = "Error",
                StatusCode = HttpStatusCode.InternalServerError
            };

            Assert.NotNull(result);
            if (result is ObjectResult objectResult)
            {
                string? message = objectResult.Value?.ToString();
                HttpStatusCode? statusCode = (HttpStatusCode?)objectResult.StatusCode;
                Assert.Equal(returnDTO.Message, message);
                Assert.Equal(returnDTO.StatusCode, statusCode);
            }
        }
    }
}
