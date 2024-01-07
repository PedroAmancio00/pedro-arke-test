using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Swashbuckle.AspNetCore.Annotations;

namespace ArkeTest.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController() : ControllerBase
    {
        [Authorize]
        [HttpPost(Name = "UserController")]
        [SwaggerOperation(Summary = "Create a new user")]
        public string CreatUser()
        {
            return "teste";
        }
    }
}
