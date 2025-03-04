using Baustellen.App.Projects.Api.Models;
using Baustellen.App.Shared.Extensions;
using Baustellen.App.Shared.Models;
using NodaTime;

namespace Baustellen.App.Projects.Api.Data.Seeding;

public class ProjectDbSeeding : IDbSeeder<ProjectsDbContext>
{
    public async Task SeedAsync(ProjectsDbContext context)
    {
        if (context.Projects.Any())
        {
            return;
        }

        var project = new Project
        {
            Id = Guid.NewGuid(),
            Name = "First Test Project",
            RefNumber = "POI00001",
            Start = DateTime.UtcNow,
            ManagerEmail = "pl.user@baustellen.app",
            ManagerName = "Pl User",
            CustomerFirstName = "Test",
            CustomerLastName = "Kunde",
            CustomerStreet = "Teststrasse",
            CustomerHouseNumber = "47b",
            CustomerCity = "Bern",
            CustomerZip = "3001",
            CustomerEmail = "test@kunde.ch",
            CustomerTelefon = "+410798247636",
            ObjectStreet = "Teststrasse",
            ObjectNumber = "47a",
            ObjectCity = "Bern",
            ObjectZip = "3001",
        };

        context.Projects.Add(project);

        context.ExternalLinks.Add(new ExternalLinks
        {
            Id = Guid.NewGuid(),
            Link = @"https://isolutionsch.sharepoint.com/sites/005749/Shared%20Documents/Forms/AllItems.aspx",
            Project = project,
            Type = LinkTypeEnum.MsSharepoint
        });
        context.ExternalLinks.Add(new ExternalLinks
        {
            Id = Guid.NewGuid(),
            Link = @"https://teams.microsoft.com/l/channel/19%3A9f5e05bc66e74a34a165e9ef4b59a44e%40thread.tacv2/General?groupId=0d79e030-673d-4f32-96ae-24786243bc70",
            Project = project,
            Type = LinkTypeEnum.MsTeams
        });
        context.ExternalLinks.Add(new ExternalLinks
        {
            Id = Guid.NewGuid(),
            Link = @"https://learn.microsoft.com/en-us/ef/core/cli/dotnet",
            Project = project,
            Type = LinkTypeEnum.Website
        });

        var project2 = new Project
        {
            Id = Guid.NewGuid(),
            Name = "Wall Charger Mount",
            RefNumber = "POI00002",
            Start = DateTime.UtcNow,
            ManagerEmail = "pl.user@baustellen.app",
            ManagerName = "Pl User",
            CustomerFirstName = "Alpha",
            CustomerLastName = "Kevin",
            CustomerStreet = "Teststrasse",
            CustomerHouseNumber = "47b",
            CustomerCity = "Bern",
            CustomerZip = "3001",
            CustomerEmail = "alpha@kevine.ch",
            CustomerTelefon = "+410798247636",
            ObjectStreet = "Teststrasse",
            ObjectNumber = "47a",
            ObjectCity = "Bern",
            ObjectZip = "3001",
        };

        context.Projects.Add(project2);

        context.ExternalLinks.Add(new ExternalLinks
        {
            Id = Guid.NewGuid(),
            Link = @"https://isolutionsch.sharepoint.com/sites/005749/Shared%20Documents/Forms/AllItems.aspx",
            Project = project2,
            Type = LinkTypeEnum.MsSharepoint
        });
        context.ExternalLinks.Add(new ExternalLinks
        {
            Id = Guid.NewGuid(),
            Link = @"https://teams.microsoft.com/l/channel/19%3A9f5e05bc66e74a34a165e9ef4b59a44e%40thread.tacv2/General?groupId=0d79e030-673d-4f32-96ae-24786243bc70",
            Project = project2,
            Type = LinkTypeEnum.MsTeams
        });
        context.ExternalLinks.Add(new ExternalLinks
        {
            Id = Guid.NewGuid(),
            Link = @"https://learn.microsoft.com/en-us/ef/core/cli/dotnet",
            Project = project2,
            Type = LinkTypeEnum.Website
        });

        await context.SaveChangesAsync();
    }
}
