using Baustellen.App.Identity.Api.Data;
using Baustellen.App.Identity.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace Baustellen.App.Identity.Api.Services;

public class UserService(IdentityDbContext dbContext)
{
    public async Task<DateTime> GetLatestModificationDateAsync()
    {
        return await dbContext.Users.MaxAsync(x => x.ModifiedAt);
    }

    public async Task<IList<User>> GetUsersAsync()
    {
        return await dbContext.Users.ToListAsync();
    }

    public async Task<User?> GetUserByAsync(int id)
    {
        return await dbContext.Users.FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task<User?> GetUserByAsync(string email)
    {
        return await dbContext.Users.FirstOrDefaultAsync(x => x.Email == email);
    }
}
