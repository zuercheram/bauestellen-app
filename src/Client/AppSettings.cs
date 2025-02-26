using Baustellen.App.Client.Exceptions;

namespace Baustellen.App.Client;

public class AppSettings
{
    private const string IdGatewayEndpoint = "DownStreamApiHost";
    private const string IdIsLocalSetting = "IsLocal";    

    public static void SetPreferenc(string key, string value)
    {
        switch (key)
        {
            case IdGatewayEndpoint:
                GatewayEndpoint = value;
                break;
            case IdIsLocalSetting:
                IsLocal = bool.Parse(value);
                break;
            default:
                throw new AppSettingsException($"App is not setup to use setting {key}");
        }
    }

    public static string GatewayEndpoint
    {
        get => Preferences.Get(IdGatewayEndpoint, "localhost");
        set => Preferences.Set(IdGatewayEndpoint, value);
    }

    public static bool IsLocal
    {
        get => Preferences.Get(IdIsLocalSetting, true);
        set => Preferences.Set(IdIsLocalSetting, value);
    }
    

}
