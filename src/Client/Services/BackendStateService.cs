using Baustellen.App.Client.Helper;
using Baustellen.App.Client.Services.Base;

using Baustellen.App.Shared.Models.ViewModels;
namespace Baustellen.App.Client.Services;

public class BackendStateService(RequestProvider requestProvider) : ApiServiceBase
{
    private const string ApiUrlBase = "projects-api/api";

    public async Task<BackendStateDto> FetchBackendState()
    {
        var uri = UriHelper.CombineUri(_baseAddress, $"{ApiUrlBase}/availability");
        try
        {
            var result = await requestProvider.GetAsync<BackendStateDto>(uri);
            if (result == null)
            {
                return new BackendStateDto
                {
                    BackendAvailable = false
                };
            }
            return result;
        }
        catch (Exception ex)
        {
            return new BackendStateDto
            {
                BackendAvailable = false
            };
        }
    }
}
