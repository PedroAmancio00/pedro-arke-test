using ArkeTest.Controllers;
using ArkeTest.DTO;
using ArkeTest.DTO.User;
using ArkeTest.Services.User.IUser;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System.Net;

namespace ArkeTeste.Tests.Tests.Controllers
{
    public class UserControllerTests
    {
        private readonly UserController _userController;
        private readonly Mock<ICreateUser> _mockCreateUser;

        public UserControllerTests()
        {
            _mockCreateUser = new Mock<ICreateUser>();

            // Initialize controller
            _userController = new UserController(_mockCreateUser.Object)
            {
                ControllerContext = new ControllerContext()
                {
                    HttpContext = new DefaultHttpContext() // Mock HttpContext
                }
            };
        }

        [Fact]
        public async Task CreateOrUpdateUser_Updated_Success()
        {
            ReturnDTO returnDTO = new()
            {
                Message = "User updated",
                StatusCode = HttpStatusCode.OK
            };

            //Mocking functions
            _mockCreateUser.Setup(x => x.CreateOrUpdateUser(It.IsAny<CreateUserDTO>()))
                .ReturnsAsync(new ReturnDTO() { Message = "User updated", StatusCode = HttpStatusCode.OK });

            IActionResult result = await _userController.CreateOrUpdateUser(new CreateUserDTO());

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
        public async Task CreateOrUpdateUser_Created_Success()
        {
            ReturnDTO returnDTO = new()
            {
                Message = "User created",
                StatusCode = HttpStatusCode.Created
            };

            //Mocking functions
            _mockCreateUser.Setup(x => x.CreateOrUpdateUser(It.IsAny<CreateUserDTO>()))
                .ReturnsAsync(new ReturnDTO() { Message = "User created", StatusCode = HttpStatusCode.Created });

            IActionResult result = await _userController.CreateOrUpdateUser(new CreateUserDTO());

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
        public async Task CreateOrUpdateUser_BadRequest()
        {
            ReturnDTO returnDTO = new()
            {
                Message = "User not logged in",
                StatusCode = HttpStatusCode.BadRequest
            };

            //Mocking functions
            _mockCreateUser.Setup(x => x.CreateOrUpdateUser(It.IsAny<CreateUserDTO>()))
                .ReturnsAsync(new ReturnDTO() { Message = "User not logged in", StatusCode = HttpStatusCode.BadRequest });

            IActionResult result = await _userController.CreateOrUpdateUser(new CreateUserDTO());

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
        public async Task CreateOrUpdateUser_NotFound()
        {
            ReturnDTO returnDTO = new()
            {
                Message = "Login not found",
                StatusCode = HttpStatusCode.NotFound
            };

            //Mocking functions
            _mockCreateUser.Setup(x => x.CreateOrUpdateUser(It.IsAny<CreateUserDTO>()))
                .ReturnsAsync(new ReturnDTO() { Message = "Login not found", StatusCode = HttpStatusCode.NotFound });

            IActionResult result = await _userController.CreateOrUpdateUser(new CreateUserDTO());

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
