namespace Baustellen.App.Projects.Api.Models.ViewModels;

public class ProjectViewModel
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public int Number { get; set; }
    public string ManagerName { get; set; }
    public Guid ManagerId { get; set; }
    public DateTime Start { get; set; }
    public DateTime Commissioning { get; set; }
    public string ObjectStreet { get; set; }
    public string ObjectNumber { get; set; }
    public string ObjectZip { get; set; }
    public string ObjectCity { get; set; }
    public string Lon { get; set; }
    public string Lat { get; set; }
}
