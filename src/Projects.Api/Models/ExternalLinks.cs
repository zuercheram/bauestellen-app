using Baustellen.App.Shared.Models;

namespace Baustellen.App.Projects.Api.Models;

public class ExternalLinks
{
    public Guid Id { get; set; }
    public Project Project {get;set; }
    public string Link { get; set; }
    public LinkTypeEnum Type { get; set; }
}
