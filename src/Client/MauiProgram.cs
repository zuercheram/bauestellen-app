using Baustellen.App.Client.Services.AppEnvironment;
using Baustellen.App.Client.Services.ProjectService;
using Baustellen.App.Client.Services.RequestProvider;
using Baustellen.App.Client.Services.Settings;
using CommunityToolkit.Maui;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Maui.Controls.Hosting;
using Microsoft.Maui.Hosting;

namespace Baustellen.App.Client;

public static class MauiProgram
{
	public static MauiApp CreateMauiApp()
	{
		var builder = MauiApp.CreateBuilder();
		builder
			.UseMauiApp<App>()
			.UseMauiCommunityToolkit()
			.ConfigureFonts(fonts =>
				{
					fonts.AddFont("Quicksand_Bold.otf", "QuicksandBold");
					fonts.AddFont("Quicksand_Bold_Oblique.otf", "QuicksandBoldOblique");
					fonts.AddFont("Quicksand_Book.otf", "QuicksandBook");
					fonts.AddFont("Quicksand_Book_Oblique.otf", "QuicksandBookOblique");
					fonts.AddFont("Quicksand_Light.otf", "QuicksandLight");
					fonts.AddFont("Quicksand_Light_Oblique.otf", "QuicksandLightOblique");
				})
#if !WINDOWS
#endif
			.ConfigureHandlers()
			.RegisterAppServices()
			.RegisterModelViews()
			.RegisterViews();

#if DEBUG
        builder.Configuration.AddInMemoryCollection(AspireAppSettings.Settings);
#endif
		builder.AddAppDefaults();

		MauiApp mauiApp = builder.Build();
		mauiApp.InitOpenTelemetryServices();
		return mauiApp;
		
	}

    public static MauiAppBuilder ConfigureHandlers(this MauiAppBuilder mauiAppBuilder)
    {
#if IOS || MACCATALYST
        mauiAppBuilder.ConfigureMauiHandlers(handlers =>
        {
            handlers.AddHandler<Microsoft.Maui.Controls.CollectionView, Microsoft.Maui.Controls.Handlers.Items2.CollectionViewHandler2>();
            handlers.AddHandler<Microsoft.Maui.Controls.CarouselView, Microsoft.Maui.Controls.Handlers.Items2.CarouselViewHandler2>();
        });
#endif

        return mauiAppBuilder;
    }

    public static MauiAppBuilder RegisterAppServices(this MauiAppBuilder builder)
	{
		builder.Services.AddSingleton<ISettingsService, SettingsService>();
		builder.Services.AddSingleton<IRequestProvider>(sp =>
		{
			var debugHandler = sp.GetKeyedService<HttpMessageHandler>("DebugHttpMessageHandler");
			return new RequestProvider(debugHandler!);
		});
		builder.Services.AddSingleton<IAppEnvironmentService, AppEnvironmentService>(sp =>
		{
			var requestProvider = sp.GetRequiredService<IRequestProvider>();
			var settingsService = sp.GetRequiredService<ISettingsService>();
			var aes = new AppEnvironmentService(new ProjectService(), new MockProjectService());
			aes.UpdateDependencies(settingsService.UseMocks);
			return aes;
		});

#if DEBUG
        builder.Services.AddKeyedSingleton<HttpMessageHandler>(
            "DebugHttpMessageHandler",
            (sp, key) =>
            {
#if ANDROID
                var handler = new Xamarin.Android.Net.AndroidMessageHandler();
                handler.ServerCertificateCustomValidationCallback = (message, cert, chain, errors) =>
                {
                    if (cert != null && cert.Issuer.Equals("CN=localhost", StringComparison.OrdinalIgnoreCase))
                    {
                        return true;
                    }

                    return errors == System.Net.Security.SslPolicyErrors.None;
                };
                return handler;
#elif IOS || MACCATALYST
                var handler = new NSUrlSessionHandler
                {
                    TrustOverrideForUrl = (sender, url, trust) => url.StartsWith("https://localhost", StringComparison.OrdinalIgnoreCase),
                };
                return handler;
#else
                return null;
#endif
            });

        builder.Logging.AddDebug();
#endif

        return builder;
	}
	
	public static MauiAppBuilder RegisterModelViews(this MauiAppBuilder builder) 
	{
		return builder;
	}

	public static MauiAppBuilder RegisterViews(this MauiAppBuilder builder)
	{
		return builder;
	}
}
