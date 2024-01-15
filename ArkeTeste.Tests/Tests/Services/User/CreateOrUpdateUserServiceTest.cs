using ArkeTest.Data;
using ArkeTest.DTO;
using ArkeTest.DTO.User;
using ArkeTest.Models;
using ArkeTest.Services.Jwt.IJwt;
using ArkeTest.Services.User;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using System.Net;

namespace ArkeTeste.Tests.Tests.Services.User
{
    public class CreateOrUpdateUserServiceTest
    {
        private readonly CreateOrUpdateUserService _createOrUpdateUserService;
        private readonly MyDbContext _mockMyDbContext;
        private readonly Mock<IJwtService> _mockIJwtService;
        private readonly Mock<ILogger<CreateOrUpdateUserService>> _mockILogger;


        public CreateOrUpdateUserServiceTest()
        {
            DbContextOptions<MyDbContext> options = new DbContextOptionsBuilder<MyDbContext>()
                   .UseInMemoryDatabase(databaseName: "RefreshService_CreteOrUpdateUser")
                   .Options;
            _mockMyDbContext = new(options);
            _mockILogger = new Mock<ILogger<CreateOrUpdateUserService>>();
            _mockIJwtService = new Mock<IJwtService>();

            _createOrUpdateUserService = new(_mockMyDbContext, _mockIJwtService.Object, _mockILogger.Object);
        }

        [Fact]
        public async Task CreateOrUpdateUser_CreateUser_Success()
        {
            string guid = Guid.NewGuid().ToString();

            List<ApplicationUser> testData = new()
            {
                new ApplicationUser { Id = guid }
            };

            _mockMyDbContext.Database.EnsureDeleted();
            _mockMyDbContext.ApplicationUsers.AddRange(testData);
            _mockMyDbContext.SaveChanges();

            CreateUserDTO dto = new()
            {
                Name = "User Example",
                AddressLine1 = "Street Example",
                AddressLine2 = "Number Example"
            };

            // Mocking functions
            _mockIJwtService.Setup(x => x.GetAndDecodeJwtToken())
              .Returns(guid);

            // Getting result
            ReturnDTO result = await _createOrUpdateUserService.CreateOrUpdateUser(dto);

            ReturnDTO returnDTO = new()
            {
                Message = "User created",
                StatusCode = HttpStatusCode.Created
            };

            // Asserting
            Assert.NotNull(result);
            Assert.Equal(returnDTO.Message, result.Message);
            Assert.Equal(returnDTO.StatusCode, result.StatusCode);

            _mockIJwtService.Verify(x => x.GetAndDecodeJwtToken(), Times.Once);
        }

        [Fact]
        public async Task CreateOrUpdateUser_UpdateUser_Success()
        {
            string guid = Guid.NewGuid().ToString();

            List<ApplicationUser> testData = new()
            {
                new ApplicationUser { Id = guid }
            };

            Guid guidUser = Guid.NewGuid();

            Users user = new("Example User", guid, null, null);

            List<Users> testDataUser = new()
            {
                user
            };

            _mockMyDbContext.Database.EnsureDeleted();
            _mockMyDbContext.UserInformations.AddRange(testDataUser);
            _mockMyDbContext.ApplicationUsers.AddRange(testData);
            _mockMyDbContext.SaveChanges();

            CreateUserDTO dto = new()
            {
                Name = "User Example",
                AddressLine1 = "Street Example",
                AddressLine2 = "Number Example"
            };

            // Mocking functions
            _mockIJwtService.Setup(x => x.GetAndDecodeJwtToken())
              .Returns(guid);

            // Getting result
            ReturnDTO result = await _createOrUpdateUserService.CreateOrUpdateUser(dto);

            ReturnDTO returnDTO = new()
            {
                Message = "User updated",
                StatusCode = HttpStatusCode.OK
            };

            // Asserting
            Assert.NotNull(result);
            Assert.Equal(returnDTO.Message, result.Message);
            Assert.Equal(returnDTO.StatusCode, result.StatusCode);

            _mockIJwtService.Verify(x => x.GetAndDecodeJwtToken(), Times.Once);
        }

        [Fact]
        public async Task CreateOrUpdateUser_BadRequest()
        {
            _mockMyDbContext.Database.EnsureDeleted();

            CreateUserDTO dto = new()
            {
                Name = "User Example",
                AddressLine1 = "Street Example",
                AddressLine2 = "Number Example"
            };

            // Mocking functions
            _mockIJwtService.Setup(x => x.GetAndDecodeJwtToken())
              .Returns(null as string);

            // Getting result
            ReturnDTO result = await _createOrUpdateUserService.CreateOrUpdateUser(dto);

            ReturnDTO returnDTO = new()
            {
                Message = "User not logged in",
                StatusCode = HttpStatusCode.BadRequest
            };

            // Asserting
            Assert.NotNull(result);
            Assert.Equal(returnDTO.Message, result.Message);
            Assert.Equal(returnDTO.StatusCode, result.StatusCode);

            _mockIJwtService.Verify(x => x.GetAndDecodeJwtToken(), Times.Once);
        }

        [Fact]
        public async Task CreateOrUpdateUser_NotFound()
        {
            _mockMyDbContext.Database.EnsureDeleted();

            CreateUserDTO dto = new()
            {
                Name = "User Example",
                AddressLine1 = "Street Example",
                AddressLine2 = "Number Example"
            };

            string guid = Guid.NewGuid().ToString();

            // Mocking functions
            _mockIJwtService.Setup(x => x.GetAndDecodeJwtToken())
              .Returns(guid);

            // Getting result
            ReturnDTO result = await _createOrUpdateUserService.CreateOrUpdateUser(dto);

            ReturnDTO returnDTO = new()
            {
                Message = "Login not found",
                StatusCode = HttpStatusCode.NotFound
            };

            // Asserting
            Assert.NotNull(result);
            Assert.Equal(returnDTO.Message, result.Message);
            Assert.Equal(returnDTO.StatusCode, result.StatusCode);

            _mockIJwtService.Verify(x => x.GetAndDecodeJwtToken(), Times.Once);
        }
    }
}
