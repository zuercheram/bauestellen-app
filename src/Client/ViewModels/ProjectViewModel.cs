#nullable enable
using Baustellen.App.Client.Messages;
using Baustellen.App.Client.Models.Project;
using Baustellen.App.Client.Services.AppEnvironment;
using Baustellen.App.Client.Services.Navigation;
using Baustellen.App.Client.ViewModels.Base;
using CommunityToolkit.Mvvm.Messaging;

namespace Baustellen.App.Client.ViewModels;

public partial class ProjectViewModel : ViewModelBase
{
    private readonly IAppEnvironmentService _appEnvironmentService;
    private readonly ObservableCollectionEx<ProjectItem> _projects = new();

    private bool _initialized;

    public ProjectViewModel(INavigationService navigationService, IAppEnvironmentService appEnvironmentService) : base(navigationService)
    {
        _appEnvironmentService = appEnvironmentService;
        _projects = new ObservableCollectionEx<ProjectItem>();

        WeakReferenceMessenger.Default
            .Register<ProjectViewModel, ProjectCountChangedMessages>(
                this,
                (_, message) =>
                {
                    BadgeCount = message.Value;
                });
    }

    internal IReadOnlyList<ProjectItem> Projects = _projects;

    internal override async Task InitializeAsync()
    {
        if (_initialized) return;

        _initialized = true;

        await IsBusyFor(async () =>
        {
            var products = await _appEnvironmentService.ProjectService.GetProjectsAsync();

            _projects.ReloadData(products);
        });
    }
}
