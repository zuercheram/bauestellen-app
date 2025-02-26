namespace Baustellen.App.Shared.Models.InputModel;

public class ProjectSyncInputDto
{
    public IDictionary<Guid, long> SyncIdTimestamps { get; set; } = new Dictionary<Guid, long>();
}
