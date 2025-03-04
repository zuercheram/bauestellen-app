using Baustellen.App.Client.Services;
using Baustellen.App.Client.Services.Base;

namespace Baustellen.App.Client.Models;

public class ConnectivityModel : ModelBase
{
    private readonly BackendStateService _backendStateService;

    private bool _backendIsAvailable = false;
    private bool _isOnline = false;
    private bool _isDownloading = false;
    private bool _isUploading = false;

    public bool BackendIsAvailable {
        get => _backendIsAvailable;
        private set => SetProperty(ref _backendIsAvailable, value);
    }
    public bool IsOnline {
        get => _isOnline && _backendIsAvailable;
        private set => SetProperty(ref _isOnline, value);
    }

    public bool IsDownloading
    {
        get => _isDownloading;
        set => SetProperty(ref _isDownloading, value);
    }

    public bool IsUploading
    {
        get => _isUploading;
        set => SetProperty(ref _isUploading, value);
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

    public async Task ConnectivityCheck(bool refresh = true)
    {
        var backendState = await _backendStateService.FetchBackendState();
        BackendIsAvailable = backendState.BackendAvailable;
        IsOnline = Connectivity.NetworkAccess == NetworkAccess.Internet || Connectivity.NetworkAccess == NetworkAccess.ConstrainedInternet;
        OnPropertyChanged(nameof(BackendIsAvailable));
        OnPropertyChanged(nameof(IsOnline));
    }

    void OnConnectivityChanged(object sender, ConnectivityChangedEventArgs e)
    {
        _ = ConnectivityCheck();
    }
}
