using Baustellen.App.Domain;
using Baustellen.App.Domain.Entities;
using Baustellen.App.Shared.Models;
using Microsoft.EntityFrameworkCore;

namespace Baustellen.App.Service;
public class ProjectService(BaustellenAppDbContext dbContext)
{
    public async void CreateProject(ProjectDto project)
    {
        var entity = CopyToProject(project);
        await dbContext.Projects.AddAsync(entity);
        await dbContext.SaveChangesAsync();
    }

    public async Task<IList<ProjectDto>> GetProjects() 
    {
        return await dbContext.Projects.Select(p => CopyToProjectView(p)).ToListAsync();
    }

    public async Task<ProjectDto?> GetProject(int id)
    {
        var project = await dbContext.Projects.FindAsync(id);
        var result = project != null ? CopyToProjectView(project) : null;
        return result;
    }

    private static Project CopyToProject(ProjectDto project)
    {
        return new Project
        {
            Id = project.Id,
            Name = project.Name,
            Description = project.Description,
            StartDate = project.StartDate,
            EndDate = project.EndDate
        };
    }

    private static ProjectDto CopyToProjectView(Project project)
    {
        return new ProjectDto
        {
            Id = project.Id,
            Name = project.Name,
            Description = project.Description,
            StartDate = project.StartDate,
            EndDate = project.EndDate
        };
    }
}