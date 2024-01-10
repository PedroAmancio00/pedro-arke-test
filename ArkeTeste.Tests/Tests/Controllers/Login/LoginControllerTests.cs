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
            //Mocking services
            Mock<ICreateLoginService> mockCreateLoginService = new();
            Mock<IAccessAccountService> accessAccountService = new();
            Mock<IRefreshService> refreshService = new();
            Mock<ILogoutService> logoutService = new();
            Mock<IChangePasswordService> changePasswordService = new();
            LoginController loginController = new(mockCreateLoginService.Object,
                                                  accessAccountService.Object,
                                                  refreshService.Object,
                                                  logoutService.Object,
                                                  changePasswordService.Object);

            ReturnDTO returnDTO = new()
            {
                Message = "Login created",
                StatusCode = HttpStatusCode.Created
            };

            //Mocking functions
            mockCreateLoginService.Setup(x => x.CreateLogin(It.IsAny<CreateLoginDTO>()))
                .ReturnsAsync(new ReturnDTO() { Message = "Login created", StatusCode = HttpStatusCode.Created });

            IActionResult result = await loginController.CreateLogin(new CreateLoginDTO());

            //Asserting
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
            //Mocking services
            Mock<ICreateLoginService> mockCreateLoginService = new();
            Mock<IAccessAccountService> accessAccountService = new();
            Mock<IRefreshService> refreshService = new();
            Mock<ILogoutService> logoutService = new();
            Mock<IChangePasswordService> changePasswordService = new();
            LoginController loginController = new(mockCreateLoginService.Object,
                                                  accessAccountService.Object,
                                                  refreshService.Object,
                                                  logoutService.Object,
                                                  changePasswordService.Object);

            //Mocking functions to fail
            mockCreateLoginService.Setup(x => x.CreateLogin(It.IsAny<CreateLoginDTO>()))
                .ReturnsAsync(new ReturnDTO() { Message = "Error", StatusCode = HttpStatusCode.Conflict });

            IActionResult result = await loginController.CreateLogin(new CreateLoginDTO());

            //Asserting
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
        public async Task CreateLogin_InternalServerError()
        {
            //Mocking services
            Mock<ICreateLoginService> mockCreateLoginService = new();
            Mock<IAccessAccountService> accessAccountService = new();
            Mock<IRefreshService> refreshService = new();
            Mock<ILogoutService> logoutService = new();
            Mock<IChangePasswordService> changePasswordService = new();
            LoginController loginController = new(mockCreateLoginService.Object,
                                                  accessAccountService.Object,
                                                  refreshService.Object,
                                                  logoutService.Object,
                                                  changePasswordService.Object);

            //Mocking functions to fail
            mockCreateLoginService.Setup(x => x.CreateLogin(It.IsAny<CreateLoginDTO>()))
                .ReturnsAsync(new ReturnDTO() { Message = "Error", StatusCode = HttpStatusCode.InternalServerError });

            ReturnDTO returnDTO = new()
            {
                Message = "Error",
                StatusCode = HttpStatusCode.InternalServerError
            };

            IActionResult result = await loginController.CreateLogin(new CreateLoginDTO());

            //Asserting
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
            //Mocking services
            Mock<ICreateLoginService> mockCreateLoginService = new();
            Mock<IAccessAccountService> accessAccountService = new();
            Mock<IRefreshService> refreshService = new();
            Mock<ILogoutService> logoutService = new();
            Mock<IChangePasswordService> changePasswordService = new();
            LoginController loginController = new(mockCreateLoginService.Object,
                                                  accessAccountService.Object,
                                                  refreshService.Object,
                                                  logoutService.Object,
                                                  changePasswordService.Object);

            ReturnDTO returnDTO = new()
            {
                Message = "Successful Login",
                StatusCode = HttpStatusCode.OK
            };

            //Mocking functions
            accessAccountService.Setup(x => x.AccessAccount(It.IsAny<AccessAccountDTO>()))
                .ReturnsAsync(new ReturnDTO() { Message = "Successful Login", StatusCode = HttpStatusCode.OK });

            IActionResult result = await loginController.AccessAccount(new AccessAccountDTO());

            //Asserting
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
            //Mocking services
            Mock<ICreateLoginService> mockCreateLoginService = new();
            Mock<IAccessAccountService> accessAccountService = new();
            Mock<IRefreshService> refreshService = new();
            Mock<ILogoutService> logoutService = new();
            Mock<IChangePasswordService> changePasswordService = new();
            LoginController loginController = new(mockCreateLoginService.Object,
                                                  accessAccountService.Object,
                                                  refreshService.Object,
                                                  logoutService.Object,
                                                  changePasswordService.Object);

            //Mocking functions to fail
            accessAccountService.Setup(x => x.AccessAccount(It.IsAny<AccessAccountDTO>()))
                .ReturnsAsync(new ReturnDTO() { Message = "Email or Password is wrong", StatusCode = HttpStatusCode.NotFound });

            IActionResult result = await loginController.AccessAccount(new AccessAccountDTO());

            ReturnDTO returnDTO = new()
            {
                Message = "Email or Password is wrong",
                StatusCode = HttpStatusCode.NotFound
            };

            //Asserting
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
        public async Task AccessAccount_InternalServerError()
        {
            //Mocking services
            Mock<ICreateLoginService> mockCreateLoginService = new();
            Mock<IAccessAccountService> accessAccountService = new();
            Mock<IRefreshService> refreshService = new();
            Mock<ILogoutService> logoutService = new();
            Mock<IChangePasswordService> changePasswordService = new();
            LoginController loginController = new(mockCreateLoginService.Object,
                                                  accessAccountService.Object,
                                                  refreshService.Object,
                                                  logoutService.Object,
                                                  changePasswordService.Object);

            //Mocking functions to fail
            accessAccountService.Setup(x => x.AccessAccount(It.IsAny<AccessAccountDTO>()))
                .ReturnsAsync(new ReturnDTO() { Message = "Error", StatusCode = HttpStatusCode.InternalServerError });

            IActionResult result = await loginController.AccessAccount(new AccessAccountDTO());

            ReturnDTO returnDTO = new()
            {
                Message = "Error",
                StatusCode = HttpStatusCode.InternalServerError
            };

            //Asserting
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
            //Mocking services
            Mock<ICreateLoginService> mockCreateLoginService = new();
            Mock<IAccessAccountService> accessAccountService = new();
            Mock<IRefreshService> refreshService = new();
            Mock<ILogoutService> logoutService = new();
            Mock<IChangePasswordService> changePasswordService = new();
            LoginController loginController = new(mockCreateLoginService.Object,
                                                  accessAccountService.Object,
                                                  refreshService.Object,
                                                  logoutService.Object,
                                                  changePasswordService.Object);

            //Mocking functions
            refreshService.Setup(x => x.Refresh())
                .ReturnsAsync(new ReturnDTO() { Message = "Refreshed", StatusCode = HttpStatusCode.OK });

            ReturnDTO returnDTO = new()
            {
                Message = "Refreshed",
                StatusCode = HttpStatusCode.OK
            };

            IActionResult result = await loginController.Refresh();

            //Asserting
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
            //Mocking services
            Mock<ICreateLoginService> mockCreateLoginService = new();
            Mock<IAccessAccountService> accessAccountService = new();
            Mock<IRefreshService> refreshService = new();
            Mock<ILogoutService> logoutService = new();
            Mock<IChangePasswordService> changePasswordService = new();
            LoginController loginController = new(mockCreateLoginService.Object,
                                                  accessAccountService.Object,
                                                  refreshService.Object,
                                                  logoutService.Object,
                                                  changePasswordService.Object);

            //Mocking functions to fail
            refreshService.Setup(x => x.Refresh())
                .ReturnsAsync(new ReturnDTO() { Message = "Refresh failed", StatusCode = HttpStatusCode.NotFound });

            IActionResult result = await loginController.Refresh();

            ReturnDTO returnDTO = new()
            {
                Message = "Refresh failed",
                StatusCode = HttpStatusCode.NotFound
            };

            //Asserting
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
        public async Task Refresh_InternalServerError()
        {
            //Mocking services
            Mock<ICreateLoginService> mockCreateLoginService = new();
            Mock<IAccessAccountService> accessAccountService = new();
            Mock<IRefreshService> refreshService = new();
            Mock<ILogoutService> logoutService = new();
            Mock<IChangePasswordService> changePasswordService = new();
            LoginController loginController = new(mockCreateLoginService.Object,
                                                  accessAccountService.Object,
                                                  refreshService.Object,
                                                  logoutService.Object,
                                                  changePasswordService.Object);

            //Mocking functions to fail
            refreshService.Setup(x => x.Refresh())
                .ReturnsAsync(new ReturnDTO() { Message = "Error", StatusCode = HttpStatusCode.InternalServerError });

            IActionResult result = await loginController.Refresh();

            ReturnDTO returnDTO = new()
            {
                Message = "Error",
                StatusCode = HttpStatusCode.InternalServerError
            };

            //Asserting
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
        public void Logout_Sucess()
        {
            //Mocking services
            Mock<ICreateLoginService> mockCreateLoginService = new();
            Mock<IAccessAccountService> accessAccountService = new();
            Mock<IRefreshService> refreshService = new();
            Mock<ILogoutService> logoutService = new();
            Mock<IChangePasswordService> changePasswordService = new();
            LoginController loginController = new(mockCreateLoginService.Object,
                                                  accessAccountService.Object,
                                                  refreshService.Object,
                                                  logoutService.Object,
                                                  changePasswordService.Object);

            //Mocking functions
            logoutService.Setup(x => x.Logout())
                .Returns(new ReturnDTO() { Message = "Logout succesfully", StatusCode = HttpStatusCode.OK });

            ReturnDTO returnDTO = new()
            {
                Message = "Logout succesfully",
                StatusCode = HttpStatusCode.OK
            };

            IActionResult result = loginController.Logout();

            //Asserting
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
        public void Logout_InternalServerError()
        {
            //Mocking services
            Mock<ICreateLoginService> mockCreateLoginService = new();
            Mock<IAccessAccountService> accessAccountService = new();
            Mock<IRefreshService> refreshService = new();
            Mock<ILogoutService> logoutService = new();
            Mock<IChangePasswordService> changePasswordService = new();
            LoginController loginController = new(mockCreateLoginService.Object,
                                                  accessAccountService.Object,
                                                  refreshService.Object,
                                                  logoutService.Object,
                                                  changePasswordService.Object);


            //Mocking functions to fail
            logoutService.Setup(x => x.Logout())
                .Returns(new ReturnDTO() { Message = "Logout failed", StatusCode = HttpStatusCode.InternalServerError });

            ReturnDTO returnDTO = new()
            {
                Message = "Logout failed",
                StatusCode = HttpStatusCode.InternalServerError
            };

            IActionResult result = loginController.Logout();

            //Asserting
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
        public async Task ChangePassword_Sucess()
        {
            //Mocking services
            Mock<ICreateLoginService> mockCreateLoginService = new();
            Mock<IAccessAccountService> accessAccountService = new();
            Mock<IRefreshService> refreshService = new();
            Mock<ILogoutService> logoutService = new();
            Mock<IChangePasswordService> changePasswordService = new();
            LoginController loginController = new(mockCreateLoginService.Object,
                                                  accessAccountService.Object,
                                                  refreshService.Object,
                                                  logoutService.Object,
                                                  changePasswordService.Object);
            ChangePasswordDto dto = new()
            {
                Email = "user@example.com",
                NewPassword = "Password12345!",
                OldPassword = "Password123!",
                RecoveryCode = "1234"
            };

            //Mocking functions
            changePasswordService.Setup(x => x.ChangePassword(dto))
                .ReturnsAsync(new ReturnDTO() { Message = "Password Changed", StatusCode = HttpStatusCode.OK });

            ReturnDTO returnDTO = new()
            {
                Message = "Password Changed",
                StatusCode = HttpStatusCode.OK
            };

            IActionResult result = await loginController.ChangePassword(dto);

            //Asserting
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
        public async Task ChangePassword_InternalServerError()
        {
            //Mocking services
            Mock<ICreateLoginService> mockCreateLoginService = new();
            Mock<IAccessAccountService> accessAccountService = new();
            Mock<IRefreshService> refreshService = new();
            Mock<ILogoutService> logoutService = new();
            Mock<IChangePasswordService> changePasswordService = new();
            LoginController loginController = new(mockCreateLoginService.Object,
                                                  accessAccountService.Object,
                                                  refreshService.Object,
                                                  logoutService.Object,
                                                  changePasswordService.Object);
            ChangePasswordDto dto = new()
            {
                Email = "user@example.com",
                NewPassword = "Password12345!",
                OldPassword = "Password123!",
                RecoveryCode = "1234"
            };

            //Mocking functions to fail
            changePasswordService.Setup(x => x.ChangePassword(dto))
                .ReturnsAsync(new ReturnDTO() { Message = "Error on password change", StatusCode = HttpStatusCode.InternalServerError });

            ReturnDTO returnDTO = new()
            {
                Message = "Error on password change",
                StatusCode = HttpStatusCode.InternalServerError
            };

            IActionResult result = await loginController.ChangePassword(dto);

            //Asserting
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
        public async Task ChangePassword_NotFoundError()
        {
            //Mocking services
            Mock<ICreateLoginService> mockCreateLoginService = new();
            Mock<IAccessAccountService> accessAccountService = new();
            Mock<IRefreshService> refreshService = new();
            Mock<ILogoutService> logoutService = new();
            Mock<IChangePasswordService> changePasswordService = new();
            LoginController loginController = new(mockCreateLoginService.Object,
                                                  accessAccountService.Object,
                                                  refreshService.Object,
                                                  logoutService.Object,
                                                  changePasswordService.Object);


            ChangePasswordDto dto = new()
            {
                Email = "user@example.com",
                NewPassword = "Password12345!",
                OldPassword = "Password123!",
                RecoveryCode = "1234"
            };

            //Mocking functions to fail
            changePasswordService.Setup(x => x.ChangePassword(dto))
                .ReturnsAsync(new ReturnDTO() { Message = "Email or Old Password or Verification Code is wrong", StatusCode = HttpStatusCode.NotFound });

            ReturnDTO returnDTO = new()
            {
                Message = "Email or Old Password or Verification Code is wrong",
                StatusCode = HttpStatusCode.NotFound
            };

            IActionResult result = await loginController.ChangePassword(dto);

            //Asserting
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
