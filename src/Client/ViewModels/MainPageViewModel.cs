using Baustellen.App.Client.Data.Entities;
using Baustellen.App.Client.Helper;
using Baustellen.App.Client.Models;
using Baustellen.App.Client.Views;
using Baustellen.App.Shared.Constants;
using System.ComponentModel;

namespace Baustellen.App.Client.ViewModels;

public partial class MainPageViewModel : ViewModelBase
{
    private readonly ProjectModel _projectModel;
    private readonly AuthUserModel _authUser;
    private readonly ConnectivityModel _connectivityModel;

    private bool _initialized;

    public IReadOnlyList<Project> OnlineProjects { get => _projectModel.RemoteProjects; }
    public IReadOnlyList<Project> OfflineProjects { get => _projectModel.OfflineProjects; }

    public MainPageViewModel(
        AuthUserModel authUser,
        ConnectivityModel connectivity,
        ProjectModel projectModel)
    {
        _connectivityModel = connectivity;
        _authUser = authUser;
        _projectModel = projectModel;

        _connectivityModel.PropertyChanged += ConnectivityModel_PropertyChanged;
        _projectModel.PropertyChanged += OnProjectModelPropertyChanged;
    }

    ~MainPageViewModel()
    {
        _connectivityModel.PropertyChanged -= ConnectivityModel_PropertyChanged;
        _projectModel.PropertyChanged -= OnProjectModelPropertyChanged;
    }

    public bool IsOnline { get => _connectivityModel.IsOnline; }

    public Project SelectedProject { get; set; }

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
    private async Task SelectProjectAsync()
    {
        if (SelectedProject == null)
        {
            return;
        }
        var navigationParameter = new Dictionary<string, object>
        {
            { AppConstants.ProjecEditEntity, SelectedProject },
            { AppConstants.IsRemoteProjectEdit, !OfflineProjects.Any(x => x.Id == SelectedProject.Id)}
        };
        await Navigation.NavigateToAsync(nameof(EditProjectPage), navigationParameter);
    }

    private void ConnectivityModel_PropertyChanged(object? sender, PropertyChangedEventArgs e)
    {
        if (e.PropertyName == nameof(_connectivityModel.IsOnline))
        {
            OnPropertyChanged(nameof(IsOnline));
            if (_connectivityModel.IsOnline)
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
