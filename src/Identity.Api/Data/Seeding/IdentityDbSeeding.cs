using Baustellen.App.Identity.Api.Models;
using Baustellen.App.Shared.Extensions;

namespace Baustellen.App.Identity.Api.Data.Seeding;

public class IdentityDbSeeding : IDbSeeder<IdentityDbContext>
{
    public async Task SeedAsync(IdentityDbContext context)
    {
        if (context.Users.Any())
        {
            return;
        }

        var fieldworker = new User
        {
            Email = "field.user@baustellen.app",
            PrincipalName = "field.user@zuercheram1.onmicrosoft.com",
            FirstName = "User",
            LastName = "Test",
            Role = Shared.Constants.AppRoleEnum.FieldWorker
        };

        context.Users.Add(fieldworker);

        var projectlead = new User
        {
            Email = "pl.user@baustellen.app",
            PrincipalName = "pl.user@zuercheram1.onmicrosoft.com",
            FirstName = "PL",
            LastName = "Test",
            Role = Shared.Constants.AppRoleEnum.ProjectLead
        };

        context.Users.Add(projectlead);

        var backoffice = new User
        {
            Email = "backoffice.user@baustellen.app",
            PrincipalName = "backoffice.user@zuercheram1.onmicrosoft.com",
            FirstName = "Backoffice",
            LastName = "Test",
            Role = Shared.Constants.AppRoleEnum.BackOffice
        };

        context.Users.Add(backoffice);

        await context.SystemSaveChangesAsync();
    }
}
