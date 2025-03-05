using Baustellen.App.Client.Helper;
using Baustellen.App.Client.Models;
using Baustellen.App.Client.Services;

namespace Baustellen.App.Client;

public partial class App : Application
{
    private readonly AuthUserModel _authUser;
    private readonly SyncingService _syncingService;
    private readonly ConnectivityModel _connectivityModel;

    public App(AuthUserModel authUser, ConnectivityModel connectivityModel, AppUserModel user, SyncingService syncingService)
    {
        _authUser = authUser;
        _connectivityModel = connectivityModel;
        _syncingService = syncingService;

        InitializeComponent();

        InitApp();

        _connectivityModel.ConnectivityStateChanged += ConnectivityModel_ConnectivityStateChanged;
    }

    private void ConnectivityModel_ConnectivityStateChanged(object? sender, ConnectivityStateEventArgs e)
    {
        if (e.IsOnline)
        {
            Task.Run(_syncingService.SyncAppUser);
            Task.Run(_syncingService.SyncProjects);
        }
    }

    protected override Window CreateWindow(IActivationState? activationState)
    {
        return new Window(new AppShell());
    }

    private void InitApp()
    {
        Task.Run(() => _connectivityModel.ConnectivityCheck());
    }

    protected override void OnSleep()
    {
        _connectivityModel.ConnectivityStateChanged -= ConnectivityModel_ConnectivityStateChanged;
    }

    protected override void OnResume()
    {
        _ = _connectivityModel.ConnectivityCheck();
        _connectivityModel.ConnectivityStateChanged += ConnectivityModel_ConnectivityStateChanged;
    }
}
