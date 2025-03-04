using Baustellen.App.Client.Data.Entities;
using Baustellen.App.Client.Data.Repositories;
using Baustellen.App.Client.Helper;
using Baustellen.App.Client.Services;

namespace Baustellen.App.Client.Models;

public class AppUserModel : ModelBase
{
    private readonly AppUserRepository _appUserRepository;

    private readonly ObservableCollectionEx<AppUser> _projectManagers = new ObservableCollectionEx<AppUser>();

    public IReadOnlyList<AppUser> ProjectManagers => _projectManagers;

    public AppUserModel(AppUserRepository appUserRepository)
    {
        _appUserRepository = appUserRepository;
    }

    public void FetchLocalProjectManagers()
    {
        var projectLeads = _appUserRepository.GetProjectManagers();
        _projectManagers.ReloadData(projectLeads);
        OnPropertyChanged(nameof(ProjectManagers));
    }
}
