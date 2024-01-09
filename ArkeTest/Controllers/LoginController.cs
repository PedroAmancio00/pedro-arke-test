using ArkeTest.DTO;
using ArkeTest.DTO.Login;
using ArkeTest.Services.Login.ILogin;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace ArkeTest.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class LoginController(ICreateLoginService createLoginService,
                                 IAccessAccountService accessAccountService,
                                 IRefreshService refreshService,
                                 ILogoutService logoutService) : ControllerBase
    {
        private readonly ICreateLoginService _createLoginService = createLoginService;
        private readonly IAccessAccountService _accessAccountService = accessAccountService;
        private readonly IRefreshService _refreshService = refreshService;
        private readonly ILogoutService _logoutService = logoutService;


        [HttpPost("create", Name = "CreateLogin")]
        [SwaggerOperation(Summary = "Create a new login", Description = "Creates a Login for the authentication")]
        [SwaggerResponse(201, "Login created")]
        [SwaggerResponse(409, "Erros that ocurred, probably email duplication")]
        [SwaggerResponse(500, "Internal Server error")]
        public async Task<IActionResult> CreateLogin([FromBody] CreateLoginDTO createLoginDTO)
        {
            ReturnDTO dto = await _createLoginService.CreateLogin(createLoginDTO);

            return StatusCode((int)dto.StatusCode, dto.Message);
        }

        [HttpPost(Name = "AccessAccount")]
        [SwaggerOperation(Summary = "Access user login", Description = "Access user login verifying information")]
        [SwaggerResponse(200, "Login Successful")]
        [SwaggerResponse(409, "Email not found or wrong password")]
        [SwaggerResponse(500, "Internal Server error")]
        public async Task<IActionResult> AccessAccount([FromBody] AccessAccountDTO accessAccount)
        {
            ReturnDTO dto = await _accessAccountService.AccessAccount(accessAccount);

            return StatusCode((int)dto.StatusCode, dto.Message);
        }

        [HttpGet("Refresh", Name = "Refresh")]
        [SwaggerOperation(Summary = "Refresh JWT", Description = "Refresh the JWT token")]
        [SwaggerResponse(200, "Login Successful")]
        [SwaggerResponse(404, "Refresh Token not found on database or cookie")]
        [SwaggerResponse(500, "Internal Server error")]
        public async Task<IActionResult> Refresh()
        {
            ReturnDTO dto = await _refreshService.Refresh();

            return StatusCode((int)dto.StatusCode, dto.Message);
        }

        [HttpGet("Logout", Name = "Logout")]
        [SwaggerOperation(Summary = "Logout", Description = "Log the user out, excluding both cookies")]
        [SwaggerResponse(200, "Logout Successful")]
        [SwaggerResponse(500, "Internal Server error")]
        public IActionResult Logout()
        {
            ReturnDTO dto = _logoutService.Logout();

            return StatusCode((int)dto.StatusCode, dto.Message);
        }

    }
}
