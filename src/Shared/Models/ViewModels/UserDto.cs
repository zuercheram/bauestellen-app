using Baustellen.App.Shared.Constants;

namespace Baustellen.App.Shared.Models.ViewModels;

public class UserDto
{
    public string Email { get; set; } = string.Empty;
    public string PrincipalName { get; set; } = string.Empty;
    public AppRoleEnum Role { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
}
