using Baustellen.App.Shared.Models.Base;

namespace Baustellen.App.Projects.Api.Models;

public class Project : TrackingEntityBase
{
    public required Guid Id { get; set; }
    public string? Name { get; set; }
    public string RefNumber { get; set; }
    public string? ManagerName { get; set; }
    public string? ManagerEmail { get; set; }
    public DateTime Start { get; set; }
    public DateTime? Commissioning { get; set; }
    public string? CustomerLastName { get; set; }
    public string? CustomerFirstName { get; set; }
    public string? CustomerStreet { get; set; }
    public string? CustomerHouseNumber { get; set; }
    public string? CustomerZip { get; set; }
    public string? CustomerCity { get; set; }
    public string? CustomerTelefon { get; set; }
    public string? CustomerEmail { get; set; }
    public string? ObjectStreet { get; set; }
    public string? ObjectNumber { get; set; }
    public string? ObjectZip { get; set; }
    public string? ObjectCity { get; set; }
    public string? Lon { get; set; }
    public string? Lat { get; set; }
    public IList<ExternalLinks> ExternalLinks { get; set; } = new List<ExternalLinks>();
}