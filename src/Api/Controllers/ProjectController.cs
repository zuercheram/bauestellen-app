using Baustellen.App.Service;
using Microsoft.AspNetCore.Mvc;

namespace Baustellen.App.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProjectController(
    ProjectService projectService
) : ControllerBase 
{

    [HttpGet()]
    public async Task<IActionResult> GetProjects()
    {
        var projects = await projectService.GetProjects();
        return Ok(projects);
    }
}