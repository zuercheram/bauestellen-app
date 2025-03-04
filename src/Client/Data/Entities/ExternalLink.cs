using Baustellen.App.Shared.Models;
using Baustellen.App.Shared.Models.InputModel;
using Baustellen.App.Shared.Models.ViewModels;
using SQLite;

namespace Baustellen.App.Client.Data.Entities;

[Table("external-link")]
public class ExternalLink
{
    [PrimaryKey]
    public Guid Id { get; set; }
    public string Link { get; set; }
    public LinkTypeEnum Type { get; set; }
    public Guid ProjectId { get; set; }

    public static ExternalLink CopyToExternalLink(ExternalLinkViewDto source, Guid projectId)
    {
        return new ExternalLink
        {
            Id = source.Id,
            Link = source.Link,
            ProjectId = projectId,
            Type = source.Type,
        };
    }

    public static ExternalLinkInputDto CopyToInputExternalLink(ExternalLink source)
    {
        return new ExternalLinkInputDto
        {
            Id = source.Id,
            Link = source.Link,
            LinkType = source.Type,
        };
    }
}
