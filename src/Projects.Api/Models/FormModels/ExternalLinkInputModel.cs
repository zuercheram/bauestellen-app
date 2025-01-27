namespace Baustellen.App.Projects.Api.Models.FormModels;

public class ExternalLinkInputModel
{
    public Guid ProjectId { get; set; }
    public string Link { get; set; }
    public LinkTypeEnum Type { get; set; }
}
