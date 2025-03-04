using Baustellen.App.Client.Models;
using Baustellen.App.Client.Services;
using Baustellen.App.Client.ViewModels;
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

        _connectivityModel.PropertyChanging += ConnectivityModel_PropertyChanging;
    }

    private void ConnectivityModel_PropertyChanging(object? sender, System.ComponentModel.PropertyChangingEventArgs e)
    {
        Task.Run(_syncingService.SyncAppUser);
        Task.Run(_syncingService.SyncProjects);
    }

    protected override Window CreateWindow(IActivationState? activationState)
    {
        return new Window(new AppShell());
    }

    private void InitApp()
    {
        if(_connectivityModel.DeviceIsOnline)
        {
            _ = _authUser.SignIn().ContinueWith(async (t) =>
            {
                await _connectivityModel.ConnectivityCheck();
            });
        }
    }

    protected override void OnSleep()
    {
        SetStatusBar();
        RequestedThemeChanged -= App_RequestedThemeChanged;
        _connectivityModel.PropertyChanging -= ConnectivityModel_PropertyChanging;
    }

    protected override void OnResume()
    {
        SetStatusBar();
        RequestedThemeChanged += App_RequestedThemeChanged;
        _ = _connectivityModel.ConnectivityCheck();
        _connectivityModel.PropertyChanging += ConnectivityModel_PropertyChanging;
    }

    private void App_RequestedThemeChanged(object sender, AppThemeChangedEventArgs e)
    {
        Dispatcher.Dispatch(() => SetStatusBar());
    }

    private void SetStatusBar()
    {
        var nav = Windows[0].Page as NavigationPage;

        if (Current.RequestedTheme == AppTheme.Dark)
        {
            if (nav != null)
            {
                nav.BarBackgroundColor = Colors.Black;
                nav.BarTextColor = Colors.White;
            }
        }
        else
        {
            if (nav != null)
            {
                nav.BarBackgroundColor = Colors.White;
                nav.BarTextColor = Colors.Black;
            }
        }
    }
}
