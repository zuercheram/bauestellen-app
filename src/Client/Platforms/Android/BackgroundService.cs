using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Baustellen.App.Client.Services;

namespace Baustellen.App.Client.Platforms.Android;

[Service]
public class BackgroundService : Service
{
    private SyncingService _syncingService;

    public override void OnCreate()
    {
        base.OnCreate();

        _syncingService = IPlatformApplication.Current!.Services.GetRequiredService<SyncingService>();
    }

    [return: GeneratedEnum]
    public override StartCommandResult OnStartCommand(Intent? intent, [GeneratedEnum] StartCommandFlags flags, int startId)
    {
        _syncingService.Start();
        return StartCommandResult.Sticky;
    }

    public override IBinder? OnBind(Intent? intent) => null;

    public override void OnDestroy()
    {
        _syncingService.Stop();
        base.OnDestroy();
    }
}

[BroadcastReceiver(Enabled = true, Exported = false)]
[IntentFilter(new[] {Intent.ActionBootCompleted}, Categories = new [] { Intent.CategoryDefault })]
public class BootReceiver : BroadcastReceiver
{
    public override void OnReceive(Context context, Intent intent)
    {
        if (intent.Action.Equals(Intent.ActionBootCompleted))
        {
            var serviceIntent = new Intent(context, typeof(BackgroundService));
            context.StartService(serviceIntent);
        }
    }
}
