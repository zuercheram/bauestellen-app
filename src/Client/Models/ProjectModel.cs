using Baustellen.App.Client.Data.Entities;
using Baustellen.App.Client.Data.Repositories;
using Baustellen.App.Client.Helper;
using Baustellen.App.Client.Services;
using Baustellen.App.Shared.Models.InputModel;
using System.ComponentModel;

namespace Baustellen.App.Client.Models;

public class ProjectModel : ModelBase
{
    private readonly ConnectivityModel _connectivity;
    private readonly ProjectRepository _projectRepository;
    private readonly ExternalLinkRepository _externalLinkRepository;
    private readonly ProjectService _projectService;

    private bool _isProjectFetching = false;

    private readonly ObservableCollectionEx<Project> _remoteProjects = new ObservableCollectionEx<Project>();
    private readonly ObservableCollectionEx<Project> _offlineProjects = new ObservableCollectionEx<Project>();
    private int _pageOffset = 0;
    private int _pageSize = 20;
    private string _searchterm = string.Empty;

    public IReadOnlyList<Project> RemoteProjects => _remoteProjects;
    public IReadOnlyList<Project> OfflineProjects => _offlineProjects;

    public int PageOffset
    {
        get => _pageOffset;
        set => SetProperty(ref _pageOffset, value);
    }

    public int PageSize
    {
        get => _pageSize;
        set => SetProperty(ref _pageSize, value);
    }

    public string SearchTerm
    {
        get => _searchterm;
        set => SetProperty(ref _searchterm, value);
    }

    public ProjectModel(
        ConnectivityModel connectivity,
        ProjectRepository projectRepository,
        ExternalLinkRepository externalLinkRepository,
        ProjectService projectService
        )
    {
        _connectivity = connectivity;
        _projectRepository = projectRepository;
        _externalLinkRepository = externalLinkRepository;
        _projectService = projectService;

        PropertyChanged += OnPropertyChanged;
    }

    ~ProjectModel()
    {
        PropertyChanged -= OnPropertyChanged;
    }

    public async Task FetchOfflineProjectsAsync()
    {
        await Task.Run(() =>
        {
            _offlineProjects.ReloadData(_projectRepository.GetAll());
            OnPropertyChanged(nameof(OfflineProjects));
        });
    }

    public async Task FetchOnlineProjectsAsync()
    {
        if (!_connectivity.IsOnline || _isProjectFetching)
        {
            return;
        }
        _isProjectFetching = true;

        var remoteIds = _projectRepository.GetAll().Select(x => x.Id);
        var result = await _projectService.RequestProjectsAsync(new RequestProjectsInputDto
        {
            PageOffset = PageOffset,
            PageCount = PageSize,
            SearchTerm = SearchTerm,
            ExcludeIds = remoteIds.ToList(),
        });
        _remoteProjects.ReloadData(result.Projects.Select(Project.CopyToProject));
        OnPropertyChanged(nameof(RemoteProjects));
        _isProjectFetching = false;
    }

    public async Task MoveProjectToOfflineStoreAsync(Project project)
    {
        await Task.Run(() => _projectRepository.AddProject(project));
        foreach (var item in project.ExternalLinks)
        {
            await Task.Run(() => _externalLinkRepository.AddExternalLink(item.Id, item.Link, project.Id, item.Type));
        }
        _remoteProjects.Remove(project);
        _offlineProjects.Add(project);
    }

    public async Task MoveProjectToRemoteListAsync(Project project)
    {
        await Task.Run(() => _projectRepository.DeleteProject(project));
        foreach (var item in project.ExternalLinks)
        {
            await Task.Run(() => _externalLinkRepository.DeleteLinks(item));
        }
        _offlineProjects.Remove(project);
        _remoteProjects.Add(project);
    }

    private void OnPropertyChanged(object? sender, PropertyChangedEventArgs e)
    {
        if (e.PropertyName == nameof(SearchTerm)
            || e.PropertyName == nameof(PageOffset)
            || e.PropertyName == nameof(PageSize))
        {
            _connectivity.IsBusy = true;
            Task.Run(FetchOnlineProjectsAsync).ContinueWith((t) =>
            {
                _connectivity.IsBusy = false;
            });
        }
    }
}
