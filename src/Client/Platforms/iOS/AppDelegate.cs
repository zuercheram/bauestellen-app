using Baustellen.App.Client.Authentication.MSALClient;
using Foundation;
using Microsoft.Identity.Client;
using UIKit;

namespace Baustellen.App.Client;

[Register("AppDelegate")]
public class AppDelegate : MauiUIApplicationDelegate
{
    protected override MauiApp CreateMauiApp() => MauiProgram.CreateMauiApp();
    public override bool FinishedLaunching(UIApplication application, NSDictionary launchOptions)
    {
        // configure platform specific params
        PlatformConfig.Instance.RedirectUri = $"msal{PublicClientSingleton.Instance.MSALClientHelper.AzureAdConfig.ClientId}://auth";

        // Initialize MSAL and platformConfig is set
        IAccount existinguser = Task.Run(async () => await PublicClientSingleton.Instance.MSALClientHelper.InitializePublicClientAppAsync()).Result;

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
}
