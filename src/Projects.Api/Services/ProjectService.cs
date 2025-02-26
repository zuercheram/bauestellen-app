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
        var projectResult = await dbContext.Projects
            .Where(x =>
                (
                    x.Name.Contains(inputModel.SearchTerm, StringComparison.OrdinalIgnoreCase)
                    || x.ManagerName.Contains(inputModel.SearchTerm, StringComparison.OrdinalIgnoreCase)
                    || x.RefNumber.Contains(inputModel.SearchTerm, StringComparison.OrdinalIgnoreCase)
                )
                &&
                !inputModel.ExcludeIds.Any(id => id.Equals(x.Id))
            )
            .Skip(inputModel.PageOffset)
            .Take(inputModel.PageCount)
            .Select(x => CopyToDto(x))
            .ToListAsync();

        return new RequestProjectViewDto
        {
            PageSize = projectResult.Count,
            Projects = projectResult,
        };
    }

    public async Task<SyncProjectsViewDto> SyncProjects(ProjectSyncInputDto projectSyncInputModel)
    {
        var newBackendProjects = await dbContext.Projects
            .Where(x =>
                !projectSyncInputModel.SyncIdTimestamps.ContainsKey(x.Id)
                || projectSyncInputModel.SyncIdTimestamps.Any(s => s.Key == x.Id && s.Value < x.ModifiedAt.Ticks)
            )
            .ToListAsync();

        var outdatedBackendProjectIds = await dbContext.Projects
            .Where(x => projectSyncInputModel.SyncIdTimestamps.Any(s => s.Key == x.Id && s.Value > x.ModifiedAt.Ticks))
            .Select(x => x.Id)
            .ToListAsync();

        var projectIds = await dbContext.Projects.Select(x => x.Id).ToListAsync();

        var newFrontendProjects = projectSyncInputModel.SyncIdTimestamps
            .Where(x => !projectIds.Contains(x.Key))
            .Select(x => x.Key)
            .ToList();

        var viewModel = new SyncProjectsViewDto
        {
            NewProjects = newBackendProjects.Select(x => CopyToDto(x)).ToList(),
            OutdatedProjects = outdatedBackendProjectIds
        };

        newFrontendProjects.ForEach(x => viewModel.OutdatedProjects.Add(x));
        return viewModel;
    }

    public async Task UpdateProjects(ProjectUpdateInputDto projectUpdateInputModel)
    {
        foreach (var project in projectUpdateInputModel.UpdateProjects)
        {
            await UpdateOrCreateProject(project.Key, project.Value);
        }
        await dbContext.SaveChangesAsync();
    }

    private async Task UpdateOrCreateProject(Guid id, ProjectInputDto projectInputModel)
    {
        var project = await dbContext.Projects.FindAsync(id);
        if (project == null)
        {
            project = new Project { Id = id };
            dbContext.Projects.Add(project);
        }
        CopyToProject(ref project, projectInputModel);
        project.ExternalLinks = ProcessExternalLinks(projectInputModel.ExternalLinks, project);
    }

    private void CopyToProject(ref Project target, ProjectInputDto source)
    {
        target.Name = source.Name;
        target.RefNumber = source.RefNumber;
        target.ManagerName = source.ManagerName;
        target.ManagerEmail = source.ManagerEmail;
        target.Start = source.Start;
        target.Commissioning = source.Commissioning;
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
            ExternalLinks = project.ExternalLinks.Select(x => new ExternalLinkViewDto { Id = x.Id, Link = x.Link, Type = x.Type }).ToList()
        };
    }

    private List<ExternalLinks> ProcessExternalLinks(IList<string> externalLinks, Project project)
    {
        return externalLinks.Select(x =>
        {
            var url = new Uri(x);
            if (url.Host.Contains("teams.microsoft.com"))
            {
                return new ExternalLinks
                {
                    Link = x,
                    Type = Shared.Models.LinkTypeEnum.MsTeams,
                    Project = project
                };
            }
            else if (url.Host.Contains("sharepoint.com"))
            {
                return new ExternalLinks
                {
                    Link = x,
                    Type = Shared.Models.LinkTypeEnum.MsSharepoint,
                    Project = project
                };
            }
            else
            {
                return new ExternalLinks
                {
                    Link = x,
                    Type = Shared.Models.LinkTypeEnum.Document,
                    Project = project
                };
            }
        }).ToList();
    }
}