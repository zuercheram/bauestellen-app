using Baustellen.App.Projects.Api.Services;
using Baustellen.App.Shared.Models.InputModel;
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
        var projects = await projectService.RequestProjects(new RequestProjectsInputDto
        {
            SearchTerm = string.Empty
        });
        return Ok(projects);
    }

    [HttpPost()]
    public async Task<IActionResult> GetProjects([FromBody] RequestProjectsInputDto inputModel)
    {
        var requestResponse = await projectService.RequestProjects(inputModel);
        return Ok(requestResponse);
    }

    [HttpPost("sync")]
    public async Task<IActionResult> SyncProjects([FromBody] ProjectSyncInputDto inputModel)
    {
        var response = await projectService.SyncProjects(inputModel);
        return Ok(response);
    }

    [HttpPost("update")]
    public async Task<IActionResult> UpdateProjects([FromBody] ProjectUpdateInputDto inputModel)
    {
        await projectService.UpdateProjects(inputModel);
        return Ok();
    }

}