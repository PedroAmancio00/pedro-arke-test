using ArkeTest.Controllers;
using ArkeTest.DTO;
using ArkeTest.DTO.Login;
using ArkeTest.Services.Login.ILogin;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System.Net;

namespace ArkeTeste.Tests.Tests.Controllers.Login
{
    public class LoginControllerTests
    {
        private readonly LoginController _loginController;
        private readonly Mock<ICreateLoginService> _mockCreateLoginService;
        private readonly Mock<IAccessAccountService> _accessAccountService;
        private readonly Mock<IRefreshService> _refreshService;
        private readonly Mock<ILogoutService> _logoutService;
        private readonly Mock<IChangePasswordService> _changePasswordService;

        public LoginControllerTests()
        {
            _mockCreateLoginService = new Mock<ICreateLoginService>();
            _accessAccountService = new Mock<IAccessAccountService>();
            _refreshService = new Mock<IRefreshService>();
            _logoutService = new Mock<ILogoutService>();
            _changePasswordService = new Mock<IChangePasswordService>();

            // Initialize controller
            _loginController = new LoginController(_mockCreateLoginService.Object,
                                                  _accessAccountService.Object,
                                                  _refreshService.Object,
                                                  _logoutService.Object,
                                                  _changePasswordService.Object)
            {
                ControllerContext = new ControllerContext()
                {
                    HttpContext = new DefaultHttpContext() // Mock HttpContext
                }
            };
        }

        [Fact]
        public async Task CreateLogin_Sucess()
        {
            ReturnDTO returnDTO = new()
            {
                Message = "Login created",
                StatusCode = HttpStatusCode.Created
            };

            // Mocking functions
            _mockCreateLoginService.Setup(x => x.CreateLogin(It.IsAny<CreateLoginDTO>()))
                .ReturnsAsync(new ReturnDTO() { Message = "Login created", StatusCode = HttpStatusCode.Created });

            IActionResult result = await _loginController.CreateLogin(new CreateLoginDTO());

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
            // Mocking functions to fail
            _mockCreateLoginService.Setup(x => x.CreateLogin(It.IsAny<CreateLoginDTO>()))
                 .ReturnsAsync(new ReturnDTO() { Message = "Error", StatusCode = HttpStatusCode.Conflict });

            IActionResult result = await _loginController.CreateLogin(new CreateLoginDTO());

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
            // Mocking functions to fail
            _mockCreateLoginService.Setup(x => x.CreateLogin(It.IsAny<CreateLoginDTO>()))
                .ReturnsAsync(new ReturnDTO() { Message = "Error", StatusCode = HttpStatusCode.InternalServerError });

            ReturnDTO returnDTO = new()
            {
                Message = "Error",
                StatusCode = HttpStatusCode.InternalServerError
            };

            IActionResult result = await _loginController.CreateLogin(new CreateLoginDTO());

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
            ReturnDTO returnDTO = new()
            {
                Message = "Successful Login",
                StatusCode = HttpStatusCode.OK
            };

            // Mocking functions
            _accessAccountService.Setup(x => x.AccessAccount(It.IsAny<AccessAccountDTO>()))
                .ReturnsAsync(new ReturnDTO() { Message = "Successful Login", StatusCode = HttpStatusCode.OK });

            IActionResult result = await _loginController.AccessAccount(new AccessAccountDTO());

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
            // Mocking functions to fail
            _accessAccountService.Setup(x => x.AccessAccount(It.IsAny<AccessAccountDTO>()))
                .ReturnsAsync(new ReturnDTO() { Message = "Email or Password is wrong", StatusCode = HttpStatusCode.NotFound });

            IActionResult result = await _loginController.AccessAccount(new AccessAccountDTO());

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
            // Mocking functions to fail
            _accessAccountService.Setup(x => x.AccessAccount(It.IsAny<AccessAccountDTO>()))
                .ReturnsAsync(new ReturnDTO() { Message = "Error", StatusCode = HttpStatusCode.InternalServerError });

            IActionResult result = await _loginController.AccessAccount(new AccessAccountDTO());

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
            // Mocking functions
            _refreshService.Setup(x => x.Refresh())
                .ReturnsAsync(new ReturnDTO() { Message = "Refreshed", StatusCode = HttpStatusCode.OK });

            ReturnDTO returnDTO = new()
            {
                Message = "Refreshed",
                StatusCode = HttpStatusCode.OK
            };

            IActionResult result = await _loginController.Refresh();

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
            // Mocking functions to fail
            _refreshService.Setup(x => x.Refresh())
                .ReturnsAsync(new ReturnDTO() { Message = "Refresh failed", StatusCode = HttpStatusCode.NotFound });

            IActionResult result = await _loginController.Refresh();

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
            // Mocking functions to fail
            _refreshService.Setup(x => x.Refresh())
                .ReturnsAsync(new ReturnDTO() { Message = "Error", StatusCode = HttpStatusCode.InternalServerError });

            IActionResult result = await _loginController.Refresh();

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
            // Mocking functions
            _logoutService.Setup(x => x.Logout())
                .Returns(new ReturnDTO() { Message = "Logout succesfully", StatusCode = HttpStatusCode.OK });

            ReturnDTO returnDTO = new()
            {
                Message = "Logout succesfully",
                StatusCode = HttpStatusCode.OK
            };

            IActionResult result = _loginController.Logout();

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
            // Mocking functions to fail
            _logoutService.Setup(x => x.Logout())
                .Returns(new ReturnDTO() { Message = "Logout failed", StatusCode = HttpStatusCode.InternalServerError });

            ReturnDTO returnDTO = new()
            {
                Message = "Logout failed",
                StatusCode = HttpStatusCode.InternalServerError
            };

            IActionResult result = _loginController.Logout();

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
            ChangePasswordDto dto = new()
            {
                Email = "user@example.com",
                NewPassword = "Password12345!",
                OldPassword = "Password123!",
                RecoveryCode = "1234"
            };

            // Mocking functions
            _changePasswordService.Setup(x => x.ChangePassword(dto))
                .ReturnsAsync(new ReturnDTO() { Message = "Password Changed", StatusCode = HttpStatusCode.OK });

            ReturnDTO returnDTO = new()
            {
                Message = "Password Changed",
                StatusCode = HttpStatusCode.OK
            };

            IActionResult result = await _loginController.ChangePassword(dto);

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
            ChangePasswordDto dto = new()
            {
                Email = "user@example.com",
                NewPassword = "Password12345!",
                OldPassword = "Password123!",
                RecoveryCode = "1234"
            };

            // Mocking functions to fail
            _changePasswordService.Setup(x => x.ChangePassword(dto))
                .ReturnsAsync(new ReturnDTO() { Message = "Error on password change", StatusCode = HttpStatusCode.InternalServerError });

            ReturnDTO returnDTO = new()
            {
                Message = "Error on password change",
                StatusCode = HttpStatusCode.InternalServerError
            };

            IActionResult result = await _loginController.ChangePassword(dto);

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

            ChangePasswordDto dto = new()
            {
                Email = "user@example.com",
                NewPassword = "Password12345!",
                OldPassword = "Password123!",
                RecoveryCode = "1234"
            };

            // Mocking functions to fail
            _changePasswordService.Setup(x => x.ChangePassword(dto))
                .ReturnsAsync(new ReturnDTO() { Message = "Email or Old Password or Verification Code is wrong", StatusCode = HttpStatusCode.NotFound });

            ReturnDTO returnDTO = new()
            {
                Message = "Email or Old Password or Verification Code is wrong",
                StatusCode = HttpStatusCode.NotFound
            };

            IActionResult result = await _loginController.ChangePassword(dto);

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
