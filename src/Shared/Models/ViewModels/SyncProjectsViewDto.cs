namespace Baustellen.App.Shared.Models.ViewModels;

public class SyncProjectsViewDto
{
    public IList<ProjectViewDto> NewProjects { get; set; } = new List<ProjectViewDto>();

    public IList<Guid> OutdatedProjects { get; set; } = new List<Guid>();
}
