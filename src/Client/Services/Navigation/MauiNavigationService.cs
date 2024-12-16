
using Baustellen.App.Client.Services.AppEnvironment;

namespace Baustellen.App.Client.Services.Navigation;

internal class MauiNavigationService(IAppEnvironmentService appEnvironmentService) : INavigationService
{
    public async Task InitializeAsync()
    {
        await NavigateToAsync("//Main/Project");
    }

    public Task NavigateToAsync(string route, IDictionary<string, object> routeParameters = null)
    {
        var shellNavigation = new ShellNavigationState(route);

        return routeParameters != null
            ? Shell.Current.GoToAsync(shellNavigation, routeParameters)
            : Shell.Current.GoToAsync(shellNavigation);
    }

    public Task PopAsync()
    {
        return Shell.Current.GoToAsync("..");
    }
}
