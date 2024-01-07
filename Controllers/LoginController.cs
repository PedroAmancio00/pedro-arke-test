using ArkeTest.DTO;
using ArkeTest.DTO.Login;
using ArkeTest.Services.Login;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace ArkeTest.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class LoginController: ControllerBase
    {

        private readonly CreateLoginService _createLoginService;
        private readonly AccessAccountService _accessAccountService;

        public LoginController(CreateLoginService createLoginService, AccessAccountService accessAccountService)
        {
            _createLoginService = createLoginService;
            _accessAccountService = accessAccountService;

        }

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
        [SwaggerResponse(201, "Login Successful")]
        [SwaggerResponse(409, "Email not found or wrong password")]
        [SwaggerResponse(500, "Internal Server error")]
        public async Task<IActionResult> AccessAccount([FromBody] AccessAccountDTO accessAccount)
        {
            ReturnDTO dto = await _accessAccountService.AccessAccount(accessAccount);

            return StatusCode((int)dto.StatusCode, dto.Message);
        }
    }
}
