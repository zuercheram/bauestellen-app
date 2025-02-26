using Baustellen.App.Client.Views;

namespace Baustellen.App.Client;

public partial class AppShell : Shell
{
    public AppShell()
    {
        InitializeRouting();
        InitializeComponent();
    }

    private static void InitializeRouting()
    {
        Routing.RegisterRoute(nameof(MainPage), typeof(MainPage));
        Routing.RegisterRoute(nameof(UserProfilePage), typeof(UserProfilePage));
    }
}
