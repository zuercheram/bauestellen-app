using Baustellen.App.Projects.Api.Data;
using Baustellen.App.Projects.Api.Models;
using Baustellen.App.Shared.Models.InputModel;
using Baustellen.App.Shared.Models.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace Baustellen.App.Projects.Api.Services;

public class ProjectService(ProjectsDbContext dbContext)
{
    public async Task<RequestProjectViewDto> RequestProjects(RequestProjectsInputDto inputModel)
    {
        var searchValues = inputModel.SearchTerm.ToLower().Split(" ");

        var projectResult = await dbContext.Projects
            .Include(x => x.ExternalLinks)
            .Where(x => !inputModel.ExcludeIds.Any(id => id.Equals(x.Id)))
            .Skip(inputModel.PageOffset)
            .Take(inputModel.PageCount)
            .ToListAsync();

        return new RequestProjectViewDto
        {
            PageSize = projectResult.Count,
            Projects = projectResult.Select(CopyToDto).ToList(),
        };
    }

    public async Task<SyncProjectsViewDto> SyncProjects(ProjectSyncInputDto projectSyncInputModel)
    {
        var updatedBackendProjects = new List<Project>();
        var oudatedBackendProjects = new List<Guid>();

        foreach (var timestamp in projectSyncInputModel.SyncIdTimestamps)
        {
            var utcTimestamp = DateTime.SpecifyKind(timestamp.Value, DateTimeKind.Utc);

            var newerProject = await dbContext.Projects
                .Include(x => x.ExternalLinks)
                .FirstOrDefaultAsync(x => x.Id == timestamp.Key && x.ModifiedAt > utcTimestamp);
            if (newerProject != null)
            {
                updatedBackendProjects.Add(newerProject);
            }

            var outdatedProject = await dbContext.Projects
                .Include(x => x.ExternalLinks)
                .FirstOrDefaultAsync(x => x.Id == timestamp.Key && x.ModifiedAt < utcTimestamp);
            if (outdatedProject != null)
            {
                oudatedBackendProjects.Add(outdatedProject.Id);
            }
        }

        var projectIds = await dbContext.Projects.Select(x => x.Id).ToListAsync();

        var newFrontendProjects = projectSyncInputModel.SyncIdTimestamps
            .Where(x => !projectIds.Contains(x.Key))
            .Select(x => x.Key)
            .ToList();

        var viewModel = new SyncProjectsViewDto
        {
            NewProjects = updatedBackendProjects.Select(x => CopyToDto(x)).ToList(),
            OutdatedProjects = oudatedBackendProjects
        };

        newFrontendProjects.ForEach(x => viewModel.OutdatedProjects.Add(x));
        return viewModel;
    }

    public async Task<List<ProjectViewDto>> UpdateProjects(ProjectUpdateInputDto projectUpdateInputModel)
    {
        var updatedProjects = new List<Project>();
        foreach (var project in projectUpdateInputModel.UpdateProjects)
        {
            updatedProjects.Add(await UpdateOrCreateProject(project.Key, project.Value));
        }
        await dbContext.SaveChangesAsync();
        return updatedProjects.Select(CopyToDto).ToList();
    }

    private async Task<Project> UpdateOrCreateProject(Guid id, ProjectInputDto projectInputModel)
    {
        var project = await dbContext.Projects.Include(x => x.ExternalLinks).FirstOrDefaultAsync(x => x.Id == id);
        if (project == null)
        {
            project = new Project { Id = id };
            dbContext.Projects.Add(project);
        }
        CopyToProject(ref project, projectInputModel);
        UpdateLinks(projectInputModel.ExternalLinks, project.ExternalLinks, project);
        return project;
    }

    private void UpdateLinks(IList<ExternalLinkInputDto> inputs, IList<ExternalLinks> links, Project project)
    {
        foreach(var inputLink in inputs)
        {
            var entity = links.FirstOrDefault(x => x.Id == inputLink.Id);
            if (entity == null)
            {
                entity = new ExternalLinks { Id = inputLink.Id };
                dbContext.ExternalLinks.Add(entity);
            }
            CopyToExternalLink(ref entity, inputLink, project);
        }
        var removeables = links.Where(x => !inputs.Any(a => a.Id == x.Id));
        dbContext.ExternalLinks.RemoveRange(removeables);
    }

    private void CopyToExternalLink(ref ExternalLinks target, ExternalLinkInputDto linkInputModel, Project project)
    {
        target.Project = project;
        target.Link = linkInputModel.Link;
        target.Type = linkInputModel.LinkType;
    }

    private void CopyToProject(ref Project target, ProjectInputDto source)
    {
        target.Name = source.Name;
        target.RefNumber = source.RefNumber;
        target.ManagerName = source.ManagerName;
        target.ManagerEmail = source.ManagerEmail;
        target.Start = DateTime.SpecifyKind(source.Start, DateTimeKind.Utc);
        target.Commissioning = source.Commissioning.HasValue ? DateTime.SpecifyKind(source.Commissioning.Value, DateTimeKind.Utc) : null ;
        target.CustomerCity = source.CustomerCity;
        target.CustomerLastName = source.CustomerLastName;
        target.CustomerFirstName = source.CustomerFirstName;
        target.CustomerEmail = source.CustomerEmail;
        target.CustomerHouseNumber = source.CustomerHouseNumber;
        target.CustomerStreet = source.CustomerStreet;
        target.CustomerTelefon = source.CustomerTelefon;
        target.CustomerZip = source.CustomerZip;
        target.CustomerEmail = source.CustomerEmail;
        target.ObjectStreet = source.ObjectStreet;
        target.ObjectNumber = source.ObjectNumber;
        target.ObjectZip = source.ObjectZip;
        target.ObjectCity = source.ObjectCity;
        target.Lon = source.Lon;
        target.Lat = source.Lat;
    }

    private ProjectViewDto CopyToDto(Project project)
    {
        return new ProjectViewDto
        {
            Id = project.Id,
            Name = project.Name,
            Commissioning = project.Commissioning,
            CustomerCity = project.CustomerCity,
            CustomerLastName = project.CustomerLastName,
            CustomerFirstName = project.CustomerFirstName,
            CustomerEmail = project.CustomerEmail,
            CustomerHouseNumber = project.CustomerHouseNumber,
            CustomerStreet = project.CustomerStreet,
            CustomerTelefon = project.CustomerTelefon,
            CustomerZip = project.CustomerZip,
            Lat = project.Lat,
            Lon = project.Lon,
            ManagerEmail = project.ManagerEmail,
            ManagerName = project.ManagerName,
            ObjectCity = project.ObjectCity,
            ObjectNumber = project.ObjectNumber,
            ObjectZip = project.ObjectZip,
            ObjectStreet = project.ObjectStreet,
            RefNumber = project.RefNumber,
            Start = project.Start,
            ModifiedAt = project.ModifiedAt,
            ExternalLinks = project.ExternalLinks.Select(x => new ExternalLinkViewDto { Id = x.Id, Link = x.Link, Type = x.Type }).ToList()
        };
    }
}