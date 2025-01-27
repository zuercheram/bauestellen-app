using Baustellen.App.Shared.Models.Base;

namespace Baustellen.App.Notes.Api.Models;

public class Note : TrackingEntityBase
{
    public Guid Id { get; set; }    
    public required string AuthorName { get; set; }
    public string? Content { get; set; }
    public Guid ProjectId { get; set; }
    public IList<Image>? Images { get; set; }
}
