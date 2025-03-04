namespace Baustellen.App.Shared.Models.InputModel;

public class ProjectSyncInputDto
{
    public IDictionary<Guid, DateTime> SyncIdTimestamps { get; set; } = new Dictionary<Guid, DateTime>();
}
