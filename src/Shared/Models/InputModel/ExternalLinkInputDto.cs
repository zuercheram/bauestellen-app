namespace Baustellen.App.Shared.Models.InputModel;

public class ExternalLinkInputDto
{
    public Guid Id { get; set; }
    public string Link { get; set; }
    public LinkTypeEnum LinkType { get; set; }
}
