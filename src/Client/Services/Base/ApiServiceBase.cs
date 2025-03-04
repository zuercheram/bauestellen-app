namespace Baustellen.App.Client.Services.Base;

public class ApiServiceBase
{
    protected string _baseAddress = AppSettings.IsLocal ? DeviceInfo.Platform == DevicePlatform.Android ? "https://10.0.2.2:7276" : AppSettings.GatewayEndpoint : AppSettings.GatewayEndpoint;
}
