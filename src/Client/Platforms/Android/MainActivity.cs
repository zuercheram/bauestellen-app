using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using Baustellen.App.Client.Authentication.MSALClient;
using Baustellen.App.Client.Platforms.Android;
using Microsoft.Identity.Client;

namespace Baustellen.App.Client;

[Activity(Theme = "@style/Maui.SplashTheme", MainLauncher = true, LaunchMode = LaunchMode.SingleTop, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.UiMode | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize | ConfigChanges.Density)]
public class MainActivity : MauiAppCompatActivity
{
    protected override void OnCreate(Bundle savedInstanceState)
    {
        PlatformConfig.Instance.RedirectUri = $"msal{PublicClientSingleton.Instance.MSALClientHelper.AzureAdConfig.ClientId}://auth";
        PlatformConfig.Instance.ParentWindow = this;

        // Initialize MSAL and platformConfig is set
        _ = Task.Run(async () => await PublicClientSingleton.Instance.MSALClientHelper.InitializePublicClientAppAsync()).Result;

        Intent backgroundSync = new Intent(this, typeof(BackgroundService));
        StartService(backgroundSync);

        base.OnCreate(savedInstanceState);
        // configure platform specific params
    }

    protected override void OnActivityResult(int requestCode, Result resultCode, Intent data)
    {
        base.OnActivityResult(requestCode, resultCode, data);
        AuthenticationContinuationHelper.SetAuthenticationContinuationEventArgs(requestCode, resultCode, data);
    }
}
