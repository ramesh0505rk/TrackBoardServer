using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TrackBoard.Application.Interfaces;
using TrackBoard.Domain.Entities;

namespace TrackBoard.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private IUserService _userService;
        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost("SignIn")]
        public async Task<IActionResult> SignIn(SignInRequest request, CancellationToken cancellationToken)
        {
            return Ok(await _userService.SignIn(request, cancellationToken));
        }

        [HttpPost("SignUp")]
        public async Task<IActionResult> SignUp(SignUpRequest request, CancellationToken cancellationToken)
        {
            return Ok(await _userService.SignUp(request, cancellationToken));
        }

        [HttpGet("Exists")]
        public async Task<IActionResult> UserNameExists([FromQuery] string UserName, CancellationToken cancellationToken)
        {
            var exists = await _userService.UserNameExists(UserName, cancellationToken);
            return Ok(exists);
		}
    }
}
