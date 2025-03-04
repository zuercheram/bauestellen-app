using Baustellen.App.Client.Authentication.MSALClient;
using Baustellen.App.Client.Services;
using Baustellen.App.Shared.Models.ViewModels;
using Microsoft.Identity.Client;
using System.Text.Json;

namespace Baustellen.App.Client.Models;

public class AuthUserModel : ModelBase
{
    private readonly UserService _userService;
    private UserDto? _authUser;

    private const string _secStoreKey = "bauapp-app-role-sec-store-key";

    private bool _isLoggingIn = false;

    public UserDto? AuthenticatedUser
    {
        get => _authUser;
        set => SetProperty(ref _authUser, value);
    }
    public IAccount? MsalAuthAccount { get; set; }
    public bool IsLoggedIn { get; set; }

    public bool CanAdd
    {
        get => AuthenticatedUser != null && (AuthenticatedUser.Role == Shared.Constants.AppRoleEnum.ProjectLead || AuthenticatedUser.Role == Shared.Constants.AppRoleEnum.BackOffice);
    }

    public bool CanEdit
    {
        get => AuthenticatedUser != null && (AuthenticatedUser.Role == Shared.Constants.AppRoleEnum.ProjectLead || AuthenticatedUser.Role == Shared.Constants.AppRoleEnum.BackOffice);
    }

    public AuthUserModel(UserService userService)
    {
        _userService = userService;
    }

    public async Task SignIn()
    {
        if (_isLoggingIn)
        {
            return;
        }
        try
        {
            _isLoggingIn = true;
            await PublicClientSingleton.Instance.AcquireTokenSilentAsync().ContinueWith((t) =>
            {
                return Task.CompletedTask;
            });
            MsalAuthAccount = PublicClientSingleton.Instance.MSALClientHelper.AuthResult.Account;
            IsLoggedIn = MsalAuthAccount is not null;
            AuthenticatedUser = await FetchAuthenticatedUserData();
        }
        catch (Exception ex) { }
        finally { _isLoggingIn = false; }
    }

    public async Task SignOut()
    {
        await PublicClientSingleton.Instance.SignOutAsync().ContinueWith((t) =>
        {
            return Task.CompletedTask;
        });
        MsalAuthAccount = null;
        AuthenticatedUser = null;
        IsLoggedIn = false;
        RemoveFromLocalCache();
    }

    public async Task<UserDto> FetchAuthenticatedUserData()
    {
        var user = await GetFromLocalCacheAsync();
        if (user == null)
        {
            user = await _userService.FetchAuthUserAsync();
            await SetLocalCacheAsync(user);
        }
        return user;
    }

    private async Task<UserDto?> GetFromLocalCacheAsync()
    {

        var user = await SecureStorage.GetAsync(_secStoreKey);
        if (!string.IsNullOrEmpty(user)) {
            return JsonSerializer.Deserialize<UserDto>(user);
        }
        return null;
    }

    private void RemoveFromLocalCache()
    {
        SecureStorage.Remove(_secStoreKey);
    }

    private async Task SetLocalCacheAsync(UserDto user)
    {
        await SecureStorage.SetAsync(_secStoreKey, JsonSerializer.Serialize(user));
    }
}
