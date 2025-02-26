namespace Baustellen.App.Shared.Models.ViewModels;

public class RequestProjectViewDto
{
    public int PageSize { get; set; }
    public IList<ProjectViewDto> Projects { get; set; } = new List<ProjectViewDto>();
}
