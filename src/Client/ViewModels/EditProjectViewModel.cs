using Baustellen.App.Client.Data.Entities;
using Baustellen.App.Client.Extensions;
using Baustellen.App.Client.Models;
using Baustellen.App.Shared.Constants;
using Microsoft.Extensions.Logging;
using System.ComponentModel;

namespace Baustellen.App.Client.ViewModels;

public partial class EditProjectViewModel : ViewModelBase
{
    private readonly EditProjectModel _model;
    private readonly AppUserModel _appUserModel;

    private AppUser? _projectLead;

    public string PageTitle => _model.ProjectName;
    public bool IsOnlineProject => _model.IsOnlineProject;
    public bool IsOfflineProject => !_model.IsOnlineProject;

    public string ProjectName
    {
        get => _model.ProjectName;
        set => SetProperty(_model.ProjectName, value, (value) =>
        {
            _model.ProjectName = value;
        });
    }

    public string RefNumber
    {
        get => _model.RefNumber;
        set => SetProperty(_model.RefNumber, value, (value) =>
        {
            _model.RefNumber = value;
        });
    }

    public string CustomerLastName
    {
        get => _model.CustomerLastName;
        set => SetProperty(_model.CustomerLastName, value, (value) =>
        {
            _model.CustomerLastName = value;
        });
    }

    public string CustomerFirstName
    {
        get => _model.CustomerFirstName;
        set => SetProperty(_model.CustomerFirstName, value, (value) =>
        {
            _model.CustomerFirstName = value;
        });
    }

    public string CustomerStreet
    {
        get => _model.CustomerStreet;
        set => SetProperty(_model.CustomerStreet, value, (value) =>
        {
            _model.CustomerStreet = value;
        });
    }

    public string CustomerHouseNumber
    {
        get => _model.CustomerHouseNumber;
        set => SetProperty(_model.CustomerHouseNumber, value, (value) =>
        {
            _model.CustomerHouseNumber = value;
        });
    }

    public string CustomerZip
    {
        get => _model.CustomerZip;
        set => SetProperty(_model.CustomerZip, value, (value) =>
        {
            _model.CustomerZip = value;
        });
    }

    public string CustomerCity
    {
        get => _model.CustomerCity;
        set => SetProperty(_model.CustomerCity, value, (value) =>
        {
            _model.CustomerCity = value;
        });
    }

    public string CustomerTelefon
    {
        get => _model.CustomerTelefon;
        set => SetProperty(_model.CustomerTelefon, value, (value) =>
        {
            _model.CustomerTelefon = value;
        });
    }

    public string CustomerEmail
    {
        get => _model.CustomerEmail;
        set => SetProperty(_model.CustomerEmail, value, (value) =>
        {
            _model.CustomerEmail = value;
        });
    }

    public string ObjectStreet
    {
        get => _model.ObjectStreet;
        set => SetProperty(_model.ObjectStreet, value, (value) =>
        {
            _model.ObjectStreet = value;
        });
    }

    public string ObjectNumber
    {
        get => _model.ObjectNumber;
        set => SetProperty(_model.ObjectNumber, value, (value) =>
        {
            _model.ObjectNumber = value;
        });
    }

    public string ObjectZip
    {
        get => _model.ObjectZip;
        set => SetProperty(_model.ObjectZip, value, (value) =>
        {
            _model.ObjectZip = value;
        });
    }

    public string ObjectCity
    {
        get => _model.ObjectCity;
        set => SetProperty(_model.ObjectCity, value, (value) =>
        {
            _model.ObjectCity = value;
        });
    }

    public DateTime Start
    {
        get => _model.Start;
        set => SetProperty(_model.Start, value, (value) =>
        {
            _model.Start = value;
        });
    }

    public DateTime? Commissioning
    {
        get => _model.Commissioning;
        set => SetProperty(_model.Commissioning, value, (value) =>
        {
            _model.Commissioning = value;
        });
    }

    public string Lon
    {
        get => _model.Lon;
        set => SetProperty(_model.Lon, value, (value) =>
        {
            _model.Lon = value;
        });
    }

    public string Lat
    {
        get => _model.Lat;
        set => SetProperty(_model.Lat, value, (value) =>
        {
            _model.Lat = value;
        });
    }

    public IReadOnlyList<AppUser> ProjectLeads => _appUserModel.ProjectManagers;

    public AppUser? SelectedProjectLead
    {
        get => _projectLead;
        set
        {
            _model.ManagerEmail = value.Email;
            _model.ManagerName = $"{value.FirstName} {value.LastName}";
            _projectLead = value;
        }
    }

    public EditProjectViewModel(EditProjectModel model, AppUserModel appUserModel, ConnectivityModel connectivityModel) : base(connectivityModel)
    {
        _model = model;
        _appUserModel = appUserModel;

        _model.PropertyChanged += Model_PropertyChanged;
        _appUserModel.PropertyChanging += AppUserModel_PropertyChanging;
    }



    ~EditProjectViewModel()
    {
        _model.PropertyChanged -= Model_PropertyChanged;
        _appUserModel.PropertyChanging -= AppUserModel_PropertyChanging;
    }

    public override async Task InitializeAsync()
    {
        _appUserModel.FetchLocalProjectManagers();
        UpdateProjectLeads();
    }

    [RelayCommand]
    public async Task SaveProjectAsync()
    {
        await _model.SaveProject();
    }

    public override void ApplyQueryAttributes(IDictionary<string, object> query)
    {
        base.ApplyQueryAttributes(query);

        if (query.ContainsKey(AppConstants.ProjecEditEntity))
        {
            _model.SetProject(query.ValueAs<Project>(AppConstants.ProjecEditEntity));
        }
        if (query.ContainsKey(AppConstants.IsRemoteProjectEdit))
        {
            _model.IsOnlineProject = query.ValueAsBool(AppConstants.IsRemoteProjectEdit);
        }
    }

    private void Model_PropertyChanged(object? sender, PropertyChangedEventArgs e)
    {
        OnPropertyChanged(nameof(PageTitle));
        OnPropertyChanged(nameof(IsOnlineProject));
        OnPropertyChanged(nameof(IsOfflineProject));
        OnPropertyChanged(nameof(ProjectName));
        OnPropertyChanged(nameof(RefNumber));
        OnPropertyChanged(nameof(Start));
        OnPropertyChanged(nameof(Commissioning));
        OnPropertyChanged(nameof(CustomerFirstName));
        OnPropertyChanged(nameof(CustomerLastName));
        OnPropertyChanged(nameof(CustomerStreet));
        OnPropertyChanged(nameof(CustomerHouseNumber));
        OnPropertyChanged(nameof(CustomerZip));
        OnPropertyChanged(nameof(CustomerCity));
        OnPropertyChanged(nameof(CustomerEmail));
        OnPropertyChanged(nameof(CustomerTelefon));
        OnPropertyChanged(nameof(ObjectStreet));
        OnPropertyChanged(nameof(ObjectNumber));
        OnPropertyChanged(nameof(ObjectZip));
        OnPropertyChanged(nameof(ObjectCity));
        OnPropertyChanged(nameof(Lat));
        OnPropertyChanged(nameof(Lon));
    }

    private void AppUserModel_PropertyChanging(object? sender, System.ComponentModel.PropertyChangingEventArgs e)
    {
        if (e.PropertyName == nameof(_appUserModel.ProjectManagers))
        {
            UpdateProjectLeads();
            OnPropertyChanged(nameof(ProjectLeads));
        }
    }

    private void UpdateProjectLeads()
    {
        _projectLead = _appUserModel.ProjectManagers.FirstOrDefault(x => x.Email == _model.ManagerEmail);
        OnPropertyChanged(nameof(SelectedProjectLead));
    }
}
