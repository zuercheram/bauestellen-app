using Baustellen.App.Projects.Api.Data;
using Baustellen.App.Projects.Api.Models;
using Baustellen.App.Projects.Api.Models.FormModels;
using Baustellen.App.Projects.Api.Models.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace Baustellen.App.Projects.Api.Services;

public class ProjectService(ProjectsDbContext dbContext)
{
    public async void CreateProject(ProjectInputModel inputModel)
    {
        var project = new Project();
        project = CopyToProject(inputModel, project);
        await dbContext.Projects.AddAsync(project);
        await dbContext.SaveChangesAsync();
    }

    public async void UpdateProject(ProjectInputModel inputModel, Guid projectId)
    {
        var project = await dbContext.Projects.FindAsync(projectId);
        if (project == null)
        {
            throw new ArgumentNullException();
        }
        project = CopyToProject(inputModel, project);
        await dbContext.SaveChangesAsync();
    }

    public async Task<IList<ProjectViewModel>> GetProjects()
    {
        return await dbContext.Projects.Select(p => CopyToProjectView(p)).ToListAsync();
    }

    public async Task DeleteProject(Guid projectId)
    {
        var project = await dbContext.Projects.FindAsync(projectId);
        if (project == null)
        {
            throw new ArgumentNullException();
        }
        dbContext.Projects.Remove(project);
    }

    private static Project CopyToProject(ProjectInputModel input, Project project)
    {
        project.Name = input.Name;
        project.Number = input.Number;
        project.Start = input.Start;
        project.Commissioning = input.Commissioning;
        project.ObjectNumber = input.ObjectNumber;
        project.ObjectStreet = input.ObjectStreet;
        project.ObjectCity = input.ObjectCity;
        project.ObjectZip = input.ObjectZip;
        project.Lon = input.Lon;
        project.Lat = input.Lat;
        project.ManagerId = input.ManagerId;
        project.ManagerName = input.ManagerName;
        return project;
    }

    private static ProjectViewModel CopyToProjectView(Project project)
    {
        return new ProjectViewModel
        {
            Id = project.Id,
            Name = project.Name,
            Number = project.Number,
            ManagerName = project.ManagerName,
            ManagerId = project.ManagerId ?? Guid.Empty,
            Start = project.Start,
            Commissioning = project.Commissioning,
            ObjectCity = project.ObjectCity,
            ObjectNumber = project.ObjectNumber,
            ObjectStreet = project.ObjectStreet,
            ObjectZip = project.ObjectZip,
            Lon = project.Lon,
            Lat = project.Lat
        };
    }
}