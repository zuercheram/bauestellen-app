namespace Baustellen.App.Shared.Models.InputModel;

public class ProjectUpdateInputDto
{
    public IDictionary<Guid, ProjectInputDto> UpdateProjects { get; set; } = new Dictionary<Guid, ProjectInputDto>();
}
