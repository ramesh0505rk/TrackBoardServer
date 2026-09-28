using Microsoft.AspNetCore.Mvc;
using TrackBoard.Application.Interfaces;
using TrackBoard.Domain.Entities;

namespace TrackBoard.Api.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class OrganizationController : ControllerBase
	{
		private IOrganizationService _OrganizationService;
		public OrganizationController(IOrganizationService organizationService)
		{
			_OrganizationService = organizationService;
		}

		[HttpPost("Register")]
		public async Task<IActionResult> Register(OrganizationRegisterRequest request, CancellationToken cancellationToken)
		{
			return Ok(await _OrganizationService.Register(request, cancellationToken));
		}
	}
}
