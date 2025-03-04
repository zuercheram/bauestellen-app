using Baustellen.App.Client.Authentication.MSALClient;
using Baustellen.App.Client.Exceptions;
using Baustellen.App.Client.Helper;
using Baustellen.App.Client.Services.Base;
using Baustellen.App.Shared.Models.ViewModels;

namespace Baustellen.App.Client.Services;

public class UserService(RequestProvider requestProvider) : ApiServiceBase
{
    private const string ApiUrlBase = "identity-api/api";

    public async Task<UserDto> FetchAuthUserAsync()
    {
        var authToken = await PublicClientSingleton.Instance.AcquireTokenSilentAsync();
        if (string.IsNullOrEmpty(authToken))
        {
            throw new ServiceAuthenticationException("No Auth token found!");
        }

        var uri = UriHelper.CombineUri(_baseAddress, $"{ApiUrlBase}/user/auth");
        try
        {
            var result = await requestProvider.GetAsync<UserDto>(uri, authToken);
            if (result == null)
            {
                throw new Exception("Response for identity role was empty!");
            }
            return result;
        }
        catch (Exception ex)
        {
            throw new HttpRequestException($"Exception while fetching data occured! {ex.Message}", ex);
        }
    }

    public async Task<List<UserDto>> SyncUserAsync(DateTime latestUpdateTicks)
    {
        var authToken = await PublicClientSingleton.Instance.AcquireTokenSilentAsync();
        if (string.IsNullOrEmpty(authToken))
        {
            throw new ServiceAuthenticationException("No Auth token found!");
        }

        var uri = UriHelper.CombineUri(_baseAddress, $"{ApiUrlBase}/users/{latestUpdateTicks.Ticks}");
        try
        {
            var result = await requestProvider.GetAsync<List<UserDto>>(uri, authToken);
            if (result == null)
            {
                throw new Exception("Response for identity role was empty!");
            }
            return result;
        }
        catch (Exception ex)
        {
            throw new HttpRequestException($"Exception while fetching data occured! {ex.Message}", ex);
        }
    }
}
