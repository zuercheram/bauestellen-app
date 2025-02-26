using Baustellen.App.Client.Authentication.MSALClient;
using Baustellen.App.Client.Helper;

namespace Baustellen.App.Client.Services;

public class AppConnectivityService(RequestProvider requestProvider) : ApiServiceBase
{
    private const string ApiUrlBase = "projects-api/api";

    public async Task<string> FetchBackendState()
    {
        var authToken = await PublicClientSingleton.Instance.AcquireTokenSilentAsync();
        if (string.IsNullOrEmpty(authToken))
        {
            return "No Auth token found!";
        }

        var uri = UriHelper.CombineUri(_baseAddress, $"{ApiUrlBase}/Availability");
        try
        {
            var result = await requestProvider.GetAsync(uri, authToken);
            return result ?? "Empty Result";
        }
        catch (Exception ex)
        {
            return $"Exception occured {ex.Message}";
        }
    }
}
