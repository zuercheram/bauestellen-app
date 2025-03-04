using Baustellen.App.Shared.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Baustellen.App.Projects.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [AllowAnonymous]
    public class AvailabilityController : ControllerBase
    {
        [HttpGet]
        public IActionResult Alive()
        {
            return Ok(new BackendStateDto
            {
                BackendAvailable = true,
            });
        }
    }
}
