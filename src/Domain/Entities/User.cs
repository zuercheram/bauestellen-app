using Microsoft.EntityFrameworkCore;

namespace Baustellen.App.Domain.Entities;

[Index(nameof(Email))]
[Index(nameof(Username), IsUnique = true)]
public class User
{
    public int Id { get; set; }
    public string? Username { get; set; }
    public string? Email { get; set; }
    public string Firstname { get; set; } = string.Empty;
    public string Lastname { get; set; } = string.Empty;
}