using Baustellen.App.Client.Authentication.MSALClient;
using Baustellen.App.Client.Services;
using Foundation;
using Microsoft.Identity.Client;
using UIKit;

namespace Baustellen.App.Client;

[Register("AppDelegate")]
public class AppDelegate : MauiUIApplicationDelegate
{
    private SyncingService _syncingService;

    protected override MauiApp CreateMauiApp() => MauiProgram.CreateMauiApp();
    public override bool FinishedLaunching(UIApplication application, NSDictionary launchOptions)
    {
        // configure platform specific params
        PlatformConfig.Instance.RedirectUri = $"msal{PublicClientSingleton.Instance.MSALClientHelper.AzureAdConfig.ClientId}://auth";

        // Initialize MSAL and platformConfig is set
        IAccount existinguser = Task.Run(async () => await PublicClientSingleton.Instance.MSALClientHelper.InitializePublicClientAppAsync()).Result;

        _syncingService = IPlatformApplication.Current!.Services.GetRequiredService<SyncingService>();
        _syncingService.Start();

        return base.FinishedLaunching(application, launchOptions);
    }

    public override bool OpenUrl(UIApplication application, NSUrl url, NSDictionary options)
    {
        if (AuthenticationContinuationHelper.IsBrokerResponse(null))
        {
            // Done on different thread to allow return in no time.
            _ = Task.Factory.StartNew(() => AuthenticationContinuationHelper.SetBrokerContinuationEventArgs(url));

            return true;
        }

        else if (!AuthenticationContinuationHelper.SetAuthenticationContinuationEventArgs(url))
        {
            return false;
        }

        return true;
    }

    public override void DidEnterBackground(UIApplication application)
    {
        _syncingService.Stop();
    }

    public override void WillEnterForeground(UIApplication application)
    {
        _syncingService.Start();
    }
}
