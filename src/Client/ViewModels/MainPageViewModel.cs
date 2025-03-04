using Baustellen.App.Client.Data.Entities;
using Baustellen.App.Client.Helper;
using Baustellen.App.Client.Models;
using Baustellen.App.Client.Services;
using Baustellen.App.Client.Views;
using Baustellen.App.Shared.Constants;
using System.ComponentModel;

namespace Baustellen.App.Client.ViewModels;

public partial class MainPageViewModel : ViewModelBase
{
    private readonly ProjectModel _projectModel;
    private readonly AuthUserModel _authUser;
    private readonly SyncingService _syncingService;

    private bool _initialized;

    public IReadOnlyList<Project> OnlineProjects { get => _projectModel.RemoteProjects; }
    public IReadOnlyList<Project> OfflineProjects { get => _projectModel.OfflineProjects; }

    public MainPageViewModel(
        AuthUserModel authUser,
        ConnectivityModel connectivity,
        ProjectModel projectModel,
        SyncingService syncingService) : base(connectivity)
    {
        _authUser = authUser;
        _projectModel = projectModel;
        _syncingService = syncingService;

        ConnectivityModel.PropertyChanged += ConnectivityModel_PropertyChanged;
        _projectModel.PropertyChanged += OnProjectModelPropertyChanged;
        _authUser.PropertyChanged += AuthUser_PropertyChanged;
    }

    private void AuthUser_PropertyChanged(object? sender, PropertyChangedEventArgs e)
    {
        OnPropertyChanged(nameof(CanEdit));
        OnPropertyChanged(nameof(CanAdd));
    }

    ~MainPageViewModel()
    {
        ConnectivityModel.PropertyChanged -= ConnectivityModel_PropertyChanged;
        _projectModel.PropertyChanged -= OnProjectModelPropertyChanged;
        _authUser.PropertyChanged -= AuthUser_PropertyChanged;
    }

    public bool CanEdit => _authUser.CanEdit;
    public bool CanAdd => _authUser.CanAdd;

    public bool IsOnline { get => ConnectivityModel.IsOnline; }

    public override async Task InitializeAsync()
    {
        await IsBusyFor(CheckUserAccessAsync);
    }

    public async Task ReloadAsync()
    {
        await IsBusyFor(_projectModel.FetchOfflineProjectsAsync);
        await IsBusyFor(_projectModel.FetchOnlineProjectsAsync);
    }

    private async Task CheckUserAccessAsync()
    {
        if (!_authUser.IsLoggedIn)
        {
            var navigationParameter = new Dictionary<string, object>
            {
                { AppConstants.ClientAutoSignInRoute, true }
            };
            await Navigation.NavigateToAsync(nameof(UserProfilePage), navigationParameter);
        }
    }

    [RelayCommand]
    private async Task AddProjectAsync()
    {
        var newProject = new Project
        {
            Id = Guid.Empty
        };
        var navigationParameter = new Dictionary<string, object>
        {
            { AppConstants.ProjecEditEntity, newProject },
            { AppConstants.IsRemoteProjectEdit, false}
        };
        await Navigation.NavigateToAsync(nameof(EditProjectPage), navigationParameter);
    }

    [RelayCommand]
    private async Task SyncDataAsync()
    {
        await ConnectivityModel.ConnectivityCheck();
        await _syncingService.SyncProjects();
        await _syncingService.SyncAppUser();
        await _projectModel.FetchOnlineProjectsAsync();
    }

    [RelayCommand]
    private async Task RemoveOfflineProjectAsync(Project project)
    {
        await _projectModel.MoveProjectToRemoteListAsync(project);
    }

    [RelayCommand]
    private async Task MarkProjectAsOfflineAsync(Project project)
    {
        await _projectModel.MoveProjectToOfflineStoreAsync(project);
    }

    [RelayCommand]
    private async Task EditProjectAsync(Project project)
    {
        var navigationParameter = new Dictionary<string, object>
        {
            { AppConstants.ProjecEditEntity, project },
            { AppConstants.IsRemoteProjectEdit, !OfflineProjects.Any(x => x.Id == project.Id)}
        };
        await Navigation.NavigateToAsync(nameof(EditProjectPage), navigationParameter);
    }

    [RelayCommand]
    private async Task ViewProjectAsync(Project project)
    {
        var navigationParameter = new Dictionary<string, object>
        {
            { AppConstants.ProjecEditEntity, project }
        };
        await Navigation.NavigateToAsync(nameof(ViewProjectPage), navigationParameter);
    }

    private void ConnectivityModel_PropertyChanged(object? sender, PropertyChangedEventArgs e)
    {
        if (e.PropertyName == nameof(ConnectivityModel.IsOnline))
        {
            OnPropertyChanged(nameof(IsOnline));
            if (ConnectivityModel.IsOnline)
            {
                _ = _projectModel.FetchOnlineProjectsAsync();
            }
        }
    }

    private void OnProjectModelPropertyChanged(object? sender, PropertyChangedEventArgs e)
    {
        OnPropertyChanged(nameof(OfflineProjects));
        OnPropertyChanged(nameof(OnlineProjects));
    }
}
