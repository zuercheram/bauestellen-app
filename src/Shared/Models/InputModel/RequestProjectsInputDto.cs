namespace Baustellen.App.Shared.Models.InputModel;

public class RequestProjectsInputDto
{
    public string SearchTerm { get; set; }
    public int PageCount { get; set; } = 0;
    public int PageOffset { get; set; } = 0;
    public IList<Guid> ExcludeIds { get; set; } = new List<Guid>();
}
