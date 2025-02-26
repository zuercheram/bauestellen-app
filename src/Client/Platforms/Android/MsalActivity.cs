using Android.App;
using Microsoft.Identity.Client;
using Android.Content;

namespace Baustellen.App.Client.Platforms.Android;

[Activity(Exported = true)]
[IntentFilter(new[] { Intent.ActionView },
    Categories = new[] { Intent.CategoryBrowsable, Intent.CategoryDefault },
    DataHost = "auth",
    DataScheme = "msal3716b8b6-5f3c-45d1-ab37-534fb505789b")]
public class MsalActivity : BrowserTabActivity
{
}
