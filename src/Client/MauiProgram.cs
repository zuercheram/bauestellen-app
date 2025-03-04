using Baustellen.App.Client.Data.Repositories;
using Baustellen.App.Client.Helper;
using Baustellen.App.Client.Models;
using Baustellen.App.Client.Services;
using Baustellen.App.Client.Services.Base;
using Baustellen.App.Client.ViewModels;
using Baustellen.App.Client.Views;
using CommunityToolkit.Maui;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Net.Security;
using System.Reflection;

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
                    fonts.AddFont("FontAwesomeRegular.otf", "FontAwesome-Regular");
                    fonts.AddFont("FontAwesomeSolid.otf", "FontAwesome-Solid");
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
            .RegisterModel()
            .RegisterRepositories()
            .RegisterPages();

        RegisterSettings();
#if DEBUG

#endif
        MauiApp mauiApp = builder.Build();
        return mauiApp;

    }

    public static MauiAppBuilder ConfigureHandlers(this MauiAppBuilder mauiAppBuilder)
    {
#if IOS
        mauiAppBuilder.ConfigureMauiHandlers(handlers =>
        {
            handlers.AddHandler<Microsoft.Maui.Controls.CollectionView, Microsoft.Maui.Controls.Handlers.Items2.CollectionViewHandler2>();
            handlers.AddHandler<Microsoft.Maui.Controls.CarouselView, Microsoft.Maui.Controls.Handlers.Items2.CarouselViewHandler2>();
        });
#endif

        return mauiAppBuilder;
    }

    public static MauiAppBuilder RegisterRepositories(this MauiAppBuilder builder)
    {
        var dbPath = FileAccessHelper.GetLocalFilePath("theiabaustelle.db3");
        builder.Services.AddSingleton(s => ActivatorUtilities.CreateInstance<ExternalLinkRepository>(s, dbPath));
        builder.Services.AddSingleton(s => ActivatorUtilities.CreateInstance<ProjectRepository>(s, dbPath));
        builder.Services.AddSingleton(s => ActivatorUtilities.CreateInstance<AppUserRepository>(s, dbPath));
        return builder;
    }

    public static MauiAppBuilder RegisterAppServices(this MauiAppBuilder builder)
    {
        builder.Services.AddSingleton<BackendStateService>();
        builder.Services.AddSingleton<SyncingService>();
        builder.Services.AddSingleton<UserService>();
        builder.Services.AddSingleton<ProjectService>();
        builder.Services.AddSingleton(sp =>
        {
            var debugHandler = sp.GetKeyedService<HttpMessageHandler>("DebugHttpMessageHandler");
            return new RequestProvider(debugHandler!);
        });
        builder.Services.AddSingleton<ConnectivityModel>();

#if DEBUG
        builder.Services.AddKeyedSingleton<HttpMessageHandler>(
            "DebugHttpMessageHandler",
            (sp, key) =>
            {
#if ANDROID
                var handler = new Xamarin.Android.Net.AndroidMessageHandler();
                handler.ServerCertificateCustomValidationCallback = (message, cert, chain, errors) =>
                {
                    if (cert != null && (cert.Issuer.Equals("CN=localhost", StringComparison.OrdinalIgnoreCase) || cert.Issuer.Equals("CN=TRAEFIK DEFAULT CERT", StringComparison.OrdinalIgnoreCase) || errors == System.Net.Security.SslPolicyErrors.None))
                    {
                        return true;
                    }

                    return false;
                };
                return handler;
#elif IOS
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
        builder.Services.AddSingleton<MainPageViewModel>();
        builder.Services.AddSingleton<UserProfileViewModel>();
        builder.Services.AddSingleton<ConnectivityViewModel>();
        builder.Services.AddSingleton<EditProjectViewModel>();
        builder.Services.AddSingleton<ViewProjectViewModel>();
        return builder;
    }

    public static MauiAppBuilder RegisterModel(this MauiAppBuilder builder)
    {
        builder.Services.AddSingleton<ConnectivityModel>();
        builder.Services.AddSingleton<AuthUserModel>();
        builder.Services.AddSingleton<ProjectModel>();
        builder.Services.AddSingleton<EditProjectModel>();
        builder.Services.AddSingleton<AppUserModel>();
        return builder;
    }

    public static MauiAppBuilder RegisterPages(this MauiAppBuilder builder)
    {
        builder.Services.AddTransient<MainPage>();
        builder.Services.AddTransient<UserProfilePage>();
        builder.Services.AddTransient<EditProjectPage>();
        builder.Services.AddTransient<ViewProjectPage>();
        return builder;
    }

    public static void RegisterSettings()
    {
        var assembly = Assembly.GetExecutingAssembly();
        using var stream = assembly.GetManifestResourceStream("Baustellen.App.Client.appsettings.json");
        if (stream is null)
        {
            return;
        }
        IConfiguration AppConfiguration = new ConfigurationBuilder().AddJsonStream(stream).Build();
        var appSettings = AppConfiguration.GetSection("Settings").GetChildren().AsEnumerable();
        foreach (var setting in appSettings)
        {
            AppSettings.SetPreferenc(setting.Key, setting.Value ?? string.Empty);
        }
    }
}
