using ArkeTest.DTO;
using ArkeTest.DTO.User;
using ArkeTest.Services.Login;
using ArkeTest.Services.Login.ILogin;
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
        public async Task<IActionResult> CreateOrUpdateUser([FromBody] CreateUserDTO createUserDTO)
        {
            ReturnDTO dto = await _createUser.CreateOrUpdateUser(createUserDTO);

            return StatusCode((int)dto.StatusCode, dto.Message);
        }
    }
}
