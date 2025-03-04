using Baustellen.App.Client.Authentication.MSALClient;
using Baustellen.App.Client.Exceptions;
using Baustellen.App.Client.Helper;
using Baustellen.App.Client.Services.Base;
using Baustellen.App.Shared.Models.InputModel;
using Baustellen.App.Shared.Models.ViewModels;

namespace Baustellen.App.Client.Services;

public class ProjectService(RequestProvider requestProvider) : ApiServiceBase
{
    private const string ApiUrlBase = "projects-api/api";

    public async Task<RequestProjectViewDto> RequestProjectsAsync(RequestProjectsInputDto requestProjectsInputDto)
    {
        var authToken = await PublicClientSingleton.Instance.AcquireTokenSilentAsync();
        if (string.IsNullOrEmpty(authToken))
        {
            throw new ServiceAuthenticationException("No Auth token found!");
        }

        var uri = UriHelper.CombineUri(_baseAddress, $"{ApiUrlBase}/projects");
        try
        {
            var result = await requestProvider.PostAsync<RequestProjectsInputDto, RequestProjectViewDto>(uri, requestProjectsInputDto, authToken);
            if (result == null)
            {
                throw new Exception("Response to project request was empty!");
            }
            return result;
        }
        catch (Exception ex)
        {
            throw new HttpRequestException($"Exception while fetching data occured! {ex.Message}", ex);
        }
    }

    public async Task<SyncProjectsViewDto> RequestProjectSynchroAsync(ProjectSyncInputDto requestProjectSyncDto)
    {
        var authToken = await PublicClientSingleton.Instance.AcquireTokenSilentAsync();
        if (string.IsNullOrEmpty(authToken))
        {
            throw new ServiceAuthenticationException("No Auth token found!");
        }

        var uri = UriHelper.CombineUri(_baseAddress, $"{ApiUrlBase}/projects/sync");
        try
        {
            var result = await requestProvider.PostAsync<ProjectSyncInputDto, SyncProjectsViewDto>(uri, requestProjectSyncDto, authToken);
            if (result == null)
            {
                throw new Exception("Response to project request was empty!");
            }
            return result;
        }
        catch (Exception ex)
        {
            throw new HttpRequestException($"Exception while fetching data occured! {ex.Message}", ex);
        }
    }

    public async Task UpdateProjectsAsync(ProjectUpdateInputDto updateProjectDto)
    {
        var authToken = await PublicClientSingleton.Instance.AcquireTokenSilentAsync();
        if (string.IsNullOrEmpty(authToken))
        {
            throw new ServiceAuthenticationException("No Auth token found!");
        }

        var uri = UriHelper.CombineUri(_baseAddress, $"{ApiUrlBase}/projects/update");
        try
        {
            await requestProvider.PostAsync(uri, updateProjectDto, authToken);
        }
        catch (Exception ex)
        {
            throw new HttpRequestException($"Exception while fetching data occured! {ex.Message}", ex);
        }
    }
}
