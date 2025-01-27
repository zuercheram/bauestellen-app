namespace Baustellen.App.Projects.Api.Models.ViewModels;

public class ExternalLinkViewModel
{
    public Guid Id { get; set; }
    public string Link { get; set; }
    public LinkTypeEnum Type { get; set; }
}
