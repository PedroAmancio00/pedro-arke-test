using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace ArkeTest.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController() : ControllerBase
    {
        [Authorize]
        [HttpPost(Name = "CreatUser")]
        [SwaggerOperation(Summary = "Create a new user")]
        public IActionResult CreatUser()
        {
            return Ok();
        }
    }
}
