namespace Baustellen.App.Client.Models.Project;

class ProjectRoot
{
    public int PageIndex { get; set; }
    public int PageSize { get; set; }
    public int Count { get; set; }
    public List<ProjectItem> Data { get; set; }
}
