namespace Baustellen.App.Client.Helper;

public class Navigation
{
    public static Task NavigateToAsync(string route, IDictionary<string, object> routeParameters = null)
    {
        var shellNavigation = new ShellNavigationState(route);

        return routeParameters != null
            ? Shell.Current.GoToAsync(shellNavigation, routeParameters)
            : Shell.Current.GoToAsync(shellNavigation);
    }

    public static Task PopAsync()
    {
        return Shell.Current.GoToAsync("..");
    }
}