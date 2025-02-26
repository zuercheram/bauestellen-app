using Baustellen.App.Client.Models;

namespace Baustellen.App.Client;

public partial class App : Application
{
    private readonly AuthUserModel _authUser;

    public App(AuthUserModel authUser)
    {
        _authUser = authUser;
        InitializeComponent();

        InitApp();
    }

    protected override Window CreateWindow(IActivationState? activationState)
    {
        return new Window(new AppShell());
    }

    private void InitApp()
    {
        _ = _authUser.SignIn();
    }

    protected override void OnSleep()
    {
        SetStatusBar();
        RequestedThemeChanged -= App_RequestedThemeChanged;
    }

    protected override void OnResume()
    {
        SetStatusBar();
        RequestedThemeChanged += App_RequestedThemeChanged;
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
