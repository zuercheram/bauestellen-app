using Baustellen.App.Client.Services;
using Baustellen.App.Client.Services.Base;

namespace Baustellen.App.Client.Models;

public class ConnectivityModel : ModelBase
{
    private readonly BackendStateService _backendStateService;

    private bool _backendIsAvailable = false;
    private bool _isOnline = false;
    private bool _isBusy;

    public bool BackendIsAvailable {
        get => _backendIsAvailable;
        private set => SetProperty(ref _backendIsAvailable, value);
    }
    public bool IsOnline {
        get => _isOnline && _backendIsAvailable;
        private set => SetProperty(ref _isOnline, value);
    }

    public bool IsBusy
    {
        get => _isBusy;
        set => SetProperty(ref _isBusy, value);
    }

    public bool DeviceIsOnline { get => Connectivity.NetworkAccess == NetworkAccess.Internet || Connectivity.NetworkAccess == NetworkAccess.ConstrainedInternet; }

    public ConnectivityModel(BackendStateService backendStateService)
    {
        _backendStateService = backendStateService;

        Connectivity.ConnectivityChanged += OnConnectivityChanged;
    }

    ~ConnectivityModel()
    {
        Connectivity.ConnectivityChanged -= OnConnectivityChanged;
    }

    public async Task ConnectivityCheck()
    {
        IsOnline = Connectivity.Current.NetworkAccess == NetworkAccess.Internet || Connectivity.Current.NetworkAccess == NetworkAccess.ConstrainedInternet;
        if (!_isOnline) { BackendIsAvailable = false; }
        var backendState = await _backendStateService.FetchBackendState();
        BackendIsAvailable = backendState.BackendAvailable;
        OnPropertyChanged(nameof(BackendIsAvailable));
        OnPropertyChanged(nameof(IsOnline));
    }

    void OnConnectivityChanged(object sender, ConnectivityChangedEventArgs e)
    {
        _ = ConnectivityCheck();
    }
}
