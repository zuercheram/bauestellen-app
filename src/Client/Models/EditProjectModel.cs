using Baustellen.App.Client.Data.Entities;
using Baustellen.App.Client.Data.Repositories;
using Baustellen.App.Client.Helper;
using Baustellen.App.Client.Services;
using Baustellen.App.Shared.Helpers;
using Baustellen.App.Shared.Models.InputModel;
using NodaTime;

namespace Baustellen.App.Client.Models;

public class EditProjectModel : ModelBase
{
    private readonly ProjectRepository _projectRepository;
    private readonly ExternalLinkRepository _externalLinkRepository;
    private readonly ProjectService _projectService;
    private readonly SyncingService _syncingService;

    private Project _project;
    private bool _isOnlineProject;

    private string _projectName = string.Empty;
    private string _refNummer = string.Empty;
    private string _managerName = string.Empty;
    private string _managerEmail = string.Empty;
    private string _customerLastName = string.Empty;
    private string _customerFirstName = string.Empty;
    private string _customerEmail = string.Empty;
    private string _customerTelefon = string.Empty;
    private string _customerStreet = string.Empty;
    private string _customerCity = string.Empty;
    private string _customerZip = string.Empty;
    private string _customerHouseNumber = string.Empty;
    private string _objectStreet = string.Empty;
    private string _objectCity = string.Empty;
    private string _objectZip = string.Empty;
    private string _objectNumber = string.Empty;
    private string _lat = string.Empty;
    private string _lon = string.Empty;
    private DateTime _start = DateTime.UtcNow;
    private DateTime? _commissioning;

    private ObservableCollectionEx<ExternalLink> _externalLinks = new ObservableCollectionEx<ExternalLink>();

    public IReadOnlyList<ExternalLink> ExternalLinks => _externalLinks;

    public bool IsOnlineProject
    {
        get => _isOnlineProject;
        set => SetProperty(ref _isOnlineProject, value);
    }

    public string ProjectName
    {
        get => _projectName;
        set => SetProperty(ref _projectName, value);
    }

    public string RefNumber
    {
        get => _refNummer;
        set => SetProperty(ref _refNummer, value);
    }

    public string ManagerName
    {
        get => _managerName;
        set => SetProperty(ref _managerName, value);
    }

    public string ManagerEmail
    {
        get => _managerEmail;
        set => SetProperty(ref _managerEmail, value);
    }

    public string CustomerLastName
    {
        get => _customerLastName;
        set => SetProperty(ref _customerLastName, value);
    }

    public string CustomerFirstName
    {
        get => _customerFirstName;
        set => SetProperty(ref _customerFirstName, value);
    }

    public string CustomerStreet
    {
        get => _customerStreet;
        set => SetProperty(ref _customerStreet, value);
    }

    public string CustomerHouseNumber
    {
        get => _customerHouseNumber;
        set => SetProperty(ref _customerHouseNumber, value);
    }

    public string CustomerZip
    {
        get => _customerZip;
        set => SetProperty(ref _customerZip, value);
    }

    public string CustomerCity
    {
        get => _customerCity;
        set => SetProperty(ref _customerCity, value);
    }

    public string CustomerTelefon
    {
        get => _customerTelefon;
        set => SetProperty(ref _customerTelefon, value);
    }

    public string CustomerEmail
    {
        get => _customerEmail;
        set => SetProperty(ref _customerEmail, value);
    }

    public string ObjectStreet
    {
        get => _objectStreet;
        set => SetProperty(ref _objectStreet, value);
    }

    public string ObjectNumber
    {
        get => _objectNumber;
        set => SetProperty(ref _objectNumber, value);
    }

    public string ObjectZip
    {
        get => _objectZip;
        set => SetProperty(ref _objectZip, value);
    }

    public string ObjectCity
    {
        get => _objectCity;
        set => SetProperty(ref _objectCity, value);
    }

    public DateTime Start
    {
        get => _start;
        set => SetProperty(ref _start, value);
    }

    public DateTime? Commissioning
    {
        get => _commissioning;
        set => SetProperty(ref _commissioning, value);
    }

    public string Lon
    {
        get => _lon;
        set => SetProperty(ref _lon, value);
    }

    public string Lat
    {
        get => _lat;
        set => SetProperty(ref _lat, value);
    }

    public void SetProject(Project project)
    {
        ProjectName = project.Name;
        RefNumber = project.RefNumber;
        Start = project.Start;
        Commissioning = project.Commissioning;
        ManagerEmail = project.ManagerEmail ?? string.Empty;
        ManagerName = project.ManagerName ?? string.Empty;
        CustomerFirstName = project.CustomerFirstName ?? string.Empty;
        CustomerLastName = project.CustomerLastName ?? string.Empty;
        CustomerStreet = project.CustomerStreet ?? string.Empty;
        CustomerHouseNumber = project.CustomerHouseNumber ?? string.Empty;
        CustomerZip = project.CustomerZip ?? string.Empty;
        CustomerCity = project.CustomerCity ?? string.Empty;
        CustomerTelefon = project.CustomerTelefon ?? string.Empty;
        CustomerEmail = project.CustomerEmail ?? string.Empty;
        ObjectStreet = project.ObjectStreet ?? string.Empty;
        ObjectNumber = project.ObjectNumber ?? string.Empty;
        ObjectCity = project.ObjectCity ?? string.Empty;
        ObjectZip = project.ObjectZip ?? string.Empty;
        Lat = project.Lat ?? string.Empty;
        Lon = project.Lon ?? string.Empty;
        _project = project;
    }

    public EditProjectModel(ProjectRepository projectRepository, ExternalLinkRepository externalLinkRepository, ProjectService projectService, SyncingService syncingService)
    {
        _projectRepository = projectRepository;
        _externalLinkRepository = externalLinkRepository;
        _projectService = projectService;
        _syncingService = syncingService;
    }

    public async Task AddExternalLink(string link)
    {
        var externalLink = new ExternalLink
        {
            Id = Guid.NewGuid(),
            Link = link,
            Type = ExternalLinkHelper.GetLinkType(link)
        };
        _externalLinks.Add(externalLink);
    }

    public async Task SaveProject()
    {
        if (_isOnlineProject)
        {
            await SaveProjectRemote();
        }
        else
        {
            await SaveProjectLocal();
        }
        await Navigation.PopAsync();
    }

    private async Task SaveProjectLocal()
    {
        CopyPropertiesToProject();
        await Task.Run(() => _projectRepository.AddProject(_project));
        await _syncingService.SyncProjects();
    }

    private async Task SaveProjectRemote()
    {
        CopyPropertiesToProject();
        var updateModel = new ProjectUpdateInputDto();
        updateModel.UpdateProjects.Add(_project.Id, Project.CopyToInputDto(_project));
        await _projectService.UpdateProjectsAsync(updateModel);
    }

    private void CopyPropertiesToProject()
    {
        _project.Name = ProjectName;
        _project.RefNumber = RefNumber;
        _project.Start = Start;
        _project.Commissioning = Commissioning;
        _project.CustomerFirstName = CustomerFirstName;
        _project.CustomerLastName = CustomerLastName;
        _project.CustomerStreet = CustomerStreet;
        _project.CustomerHouseNumber = CustomerHouseNumber;
        _project.CustomerCity = CustomerCity;
        _project.CustomerZip = CustomerZip;
        _project.CustomerEmail = CustomerEmail;
        _project.CustomerTelefon = CustomerTelefon;
        _project.ObjectCity = ObjectCity;
        _project.ObjectStreet = ObjectStreet;
        _project.ObjectNumber = ObjectNumber;
        _project.ObjectZip = ObjectZip;
        _project.ManagerEmail = ManagerEmail;
        _project.ManagerName = ManagerName;
    }
}
