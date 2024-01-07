using ArkeTest.DTO;
using ArkeTest.DTO.Login;
using ArkeTest.Services.Login;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Net;

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
            ReturnJwtDTO dto = await _accessAccountService.AccessAccount(accessAccount);

            if(dto.StatusCode == HttpStatusCode.OK)
            {
                var jwtCookieOptions = new CookieOptions
                {
                    HttpOnly = true,
                    Secure = true,
                    Expires = DateTime.UtcNow.AddHours(1)
                };
               
                var refreshTokenCookieOptions = new CookieOptions
                {
                    HttpOnly = true,
                    Secure = true,
                    Expires = DateTime.UtcNow.AddDays(7)
                };
                if (dto.JwtToken != null && dto.RefreshToken != null)
                {
                    Response.Cookies.Append("jwt", dto.JwtToken, jwtCookieOptions);

                    Response.Cookies.Append("refreshToken", dto.RefreshToken, refreshTokenCookieOptions);
                }
            }
            return StatusCode((int)dto.StatusCode, dto.Message);
        }
        /*
        [HttpPost("refresh", Name = "Refresh")]
        [SwaggerOperation(Summary = "Refresh JWT", Description = "Refresh the JWT token")]
        public async Task<IActionResult> Refresh([FromHeader] string token)
        {
            ReturnJwtDTO dto = await _accessAccountService.Refresh(accessAccount);

            if (dto.StatusCode == HttpStatusCode.OK)
            {
                var jwtCookieOptions = new CookieOptions
                {
                    HttpOnly = true,
                    Secure = true,
                    Expires = DateTime.UtcNow.AddHours(1)
                };

                if (dto.JwtToken != null && dto.RefreshToken != null)
                {
                    Response.Cookies.Append("jwt", dto.JwtToken, jwtCookieOptions);
                }
            }
            return StatusCode((int)dto.StatusCode, dto.Message);
        }
        */
    }
}
