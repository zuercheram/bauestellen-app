using Baustellen.App.Projects.Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace Baustellen.App.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProjectsController(
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