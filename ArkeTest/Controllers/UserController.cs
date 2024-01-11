using ArkeTest.DTO;
using ArkeTest.DTO.User;
using ArkeTest.Services.User.IUser;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace ArkeTest.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController(ICreateUser createUser) : ControllerBase

    {
        private readonly ICreateUser _createUser = createUser;

        [Authorize]
        [HttpPost(Name = "CreateOrUpdateUser")]
        [SwaggerOperation(Summary = "Create or update user")]
        [SwaggerResponse(200, "User not logged in")]
        [SwaggerResponse(201, "User created")]
        [SwaggerResponse(400, "Erros that ocurred, probably email duplication")]
        [SwaggerResponse(404, "Login not found")]
        [SwaggerResponse(500, "Internal Server error")]
        public async Task<IActionResult> CreateOrUpdateUser([FromBody] CreateUserDTO createUserDTO)
        {
            ReturnDTO dto = await _createUser.CreateOrUpdateUser(createUserDTO);

            return StatusCode((int)dto.StatusCode, dto.Message);
        }
    }
}
