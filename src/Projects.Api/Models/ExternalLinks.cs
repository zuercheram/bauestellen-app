using Baustellen.App.Shared.Models.Base;

namespace Baustellen.App.Projects.Api.Models;

public class ExternalLinks
{
    public Guid Id { get; set; }
    public required Project Project {get;set; }
    public required string Link { get; set; }
    public LinkTypeEnum Type { get; set; }
}
