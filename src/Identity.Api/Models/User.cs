using Baustellen.App.Shared.Constants;
using Baustellen.App.Shared.Models.Base;
using System.ComponentModel.DataAnnotations;

namespace Baustellen.App.Identity.Api.Models;

public class User : TrackingEntityBase
{
    public int Id { get; set; }
    [Required]
    public string Email { get; set; } = string.Empty;
    [Required]
    public string PrincipalName { get; set; } = string.Empty;
    [Required]
    public AppRoleEnum Role { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
}
