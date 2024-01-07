using ArkeTest.DTO;
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

        public LoginController(CreateLoginService createLoginService)
        {
            _createLoginService = createLoginService;
        }

        [HttpPost(Name = "LoginController")]
        [SwaggerOperation(Summary = "Create a new login", Description = "Creates a Login for the authentication")]
        [SwaggerResponse(201, "Login created")]
        [SwaggerResponse(409, "Erros that ocurred, probably email duplication")]
        [SwaggerResponse(500, "Internal Server error")]
        public async Task<IActionResult> CreateLogin([FromBody] CreateLoginDTO createUserDTO)
        {
            ReturnDTO dto = await _createLoginService.CreateLogin(createUserDTO);

            return StatusCode((int)dto.StatusCode, dto.Message);
        }
    }
}
