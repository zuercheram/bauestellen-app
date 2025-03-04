using Baustellen.App.Shared.Models.InputModel;
using Baustellen.App.Shared.Models.ViewModels;
using NodaTime;
using SQLite;

namespace Baustellen.App.Client.Data.Entities;

[Table("projects")]
public class Project
{
    [PrimaryKey]
    public Guid Id { get; set; }
    public string Name { get; set; }
    [Unique]
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
    public DateTime ModifiedAt { get; set; }
    [Ignore]
    public IList<ExternalLink> ExternalLinks { get; set; } = new List<ExternalLink>();

    public static Project CopyToProject(ProjectViewDto source)
    {
        return new Project
        {
            Id = source.Id,
            Name = source.Name ?? string.Empty,
            Commissioning = source.Commissioning,
            CustomerCity = source.CustomerCity,
            CustomerEmail = source.CustomerEmail,
            CustomerFirstName = source.CustomerFirstName,
            CustomerHouseNumber = source.CustomerHouseNumber,
            CustomerLastName = source.CustomerLastName,
            CustomerStreet = source.CustomerStreet,
            CustomerTelefon = source.CustomerTelefon,
            CustomerZip = source.CustomerZip,
            Lat = source.Lat,
            Lon = source.Lon,
            ManagerEmail = source.ManagerEmail,
            ManagerName = source.ManagerName,
            ObjectCity = source.ObjectCity,
            ObjectNumber = source.ObjectNumber,
            ObjectStreet = source.ObjectStreet,
            ObjectZip = source.ObjectZip,
            RefNumber = source.RefNumber,
            Start = source.Start,
            ExternalLinks = source.ExternalLinks.Select(x => ExternalLink.CopyToExternalLink(x, source.Id)).ToList()
        };
    }

    public static ProjectInputDto CopyToInputDto(Project source)
    {
        return new ProjectInputDto
        {
            Name = source.Name ?? string.Empty,
            Commissioning = source.Commissioning,
            CustomerCity = source.CustomerCity,
            CustomerEmail = source.CustomerEmail,
            CustomerFirstName = source.CustomerFirstName,
            CustomerHouseNumber = source.CustomerHouseNumber,
            CustomerLastName = source.CustomerLastName,
            CustomerStreet = source.CustomerStreet,
            CustomerTelefon = source.CustomerTelefon,
            CustomerZip = source.CustomerZip,
            Lat = source.Lat,
            Lon = source.Lon,
            ManagerEmail = source.ManagerEmail,
            ManagerName = source.ManagerName,
            ObjectCity = source.ObjectCity,
            ObjectNumber = source.ObjectNumber,
            ObjectStreet = source.ObjectStreet,
            ObjectZip = source.ObjectZip,
            RefNumber = source.RefNumber,
            Start = source.Start,
            ExternalLinks = source.ExternalLinks.Select(x => ExternalLink.CopyToInputExternalLink(x)).ToList()
        };
    }
}
