using Baustellen.App.Shared.Models.Base;

namespace Baustellen.App.Projects.Api.Models;

public class Customer : TrackingEntityBase
{
    public Guid Id { get; set; }
    public required string Name { get; set; }
    public required string FirstName { get; set; }
    public string? Street { get; set; }
    public string? Number { get; set; }
    public string? Zip { get; set; }
    public string? City { get; set; }
    public string? Telefon { get; set; }
    public string? email { get; set; }
    public IList<Project>? Projects { get; set; }
}
