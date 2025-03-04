using Baustellen.App.Shared.Constants;
using Baustellen.App.Shared.Models.ViewModels;
using NodaTime;
using SQLite;

namespace Baustellen.App.Client.Data.Entities;

[Table("app-users")]
public class AppUser
{
    [PrimaryKey, AutoIncrement]
    public int Id { get; set; }
    public string Email { get; set; }
    public string PrincipalName { get; set; }
    public AppRoleEnum Role { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public DateTime ModifiedAt { get; set; }
    [Ignore]
    public string DisplayName => $"{FirstName} {LastName}";

    public static AppUser CopyToAppUser(UserDto source)
    {
        return new AppUser
        {
            Email = source.Email,
            PrincipalName = source.PrincipalName,
            Role = source.Role,
            FirstName = source.FirstName,
            LastName = source.LastName,
            ModifiedAt = source.ModifiedAt,
        };
    }
}
