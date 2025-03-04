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

        var projectlead2 = new User
        {
            Email = "plthesecond.user@baustellen.app",
            PrincipalName = "plthesecond.user@zuercheram1.onmicrosoft.com",
            FirstName = "PL the second",
            LastName = "Test 2",
            Role = Shared.Constants.AppRoleEnum.ProjectLead
        };

        context.Users.Add(projectlead2);

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
