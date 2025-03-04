using Baustellen.App.Client.Data.Entities;
using Baustellen.App.Client.Extensions;
using Baustellen.App.Client.Models;
using Baustellen.App.Shared.Constants;

namespace Baustellen.App.Client.ViewModels;

public class ViewProjectViewModel : ViewModelBase
{
    private Project _project;

    public Project Project
    {
        get => _project;
        set => SetProperty(ref _project, value);
    }

    public ViewProjectViewModel(ConnectivityModel connectivityModel) : base(connectivityModel)
    {
    }

    public override void ApplyQueryAttributes(IDictionary<string, object> query)
    {
        base.ApplyQueryAttributes(query);

        if (query.ContainsKey(AppConstants.ProjecEditEntity))
        {
            Project = query.ValueAs<Project>(AppConstants.ProjecEditEntity);
        }
    }
}
