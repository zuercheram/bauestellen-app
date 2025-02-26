using Baustellen.App.Identity.Api.Data;
using Baustellen.App.Identity.Api.Models;
using Baustellen.App.Shared.Models.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace Baustellen.App.Identity.Api.Services;

public class UserService(IdentityDbContext dbContext)
{
    public async Task<DateTime> GetLatestModificationDateAsync()
    {
        return await dbContext.Users.MaxAsync(x => x.ModifiedAt);
    }

    public async Task<IList<UserDto>> GetUsersAsync(long? timestamp)
    {
        var lastModified = await GetLastModificationAsync();
        if (!timestamp.HasValue || timestamp >= lastModified.Ticks)
        {
            return new List<UserDto>();
        }
        return await dbContext.Users.Select(x => CopyToDto(x)).ToListAsync();
    }

    public async Task<UserDto?> GetUserByAsync(int id)
    {
        var user = await dbContext.Users.FirstOrDefaultAsync(x => x.Id == id);
        return user == null ? null : CopyToDto(user);
    }

    public async Task<UserDto?> GetUserByPrincipalAsync(string principal)
    {
        var user = await dbContext.Users.FirstOrDefaultAsync(x => x.PrincipalName == principal);
        return user == null ? null : CopyToDto(user);
    }

    public async Task<string> GetUserRoleAsync(string principal)
    {
        var user = await dbContext.Users.FirstOrDefaultAsync(x => x.PrincipalName == principal);
        if (user == null)
        {
            return string.Empty;
        }
        return user.Role.ToString();
    }

    public async Task<DateTime> GetLastModificationAsync()
    {
        return await dbContext.Users.MaxAsync(x => x.ModifiedAt);
    }

    private UserDto CopyToDto(User user)
    {
        return new UserDto
        {
            Email = user.Email,
            FirstName = user.FirstName,
            LastName = user.LastName,
            PrincipalName = user.PrincipalName,
            Role = user.Role,
        };
    }
}
