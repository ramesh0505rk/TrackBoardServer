using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TrackBoard.Domain.Entities;

namespace TrackBoard.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        [HttpPost("SignIn")]
        public async Task<IActionResult> SignIn(SignInRequest request)
        {
            return Ok();
        }
    }
}
