using Baustellen.App.Client.Helper;
using Baustellen.App.Client.Models;
using Baustellen.App.Client.Views;
using Baustellen.App.Shared.Constants;

namespace Baustellen.App.Client.ViewModels;

public partial class MainPageViewModel : ViewModelBase
{


    private readonly AuthUserModel _authUser;
    private bool _initialized;

    public MainPageViewModel(AuthUserModel authUser)
    {
        _authUser = authUser;
    }

    public override async Task InitializeAsync()
    {
        if (_initialized)
        {
            return;
        }

        _initialized = true;
        await IsBusyFor(CheckUserAccess);
    }

    private async Task CheckUserAccess()
    {
        if (!_authUser.IsLoggedIn)
        {
            var navigationParameter = new Dictionary<string, object>
            {
                { AppConstants.ClientAutoSignInRoute, true }
            };
            await Navigation.NavigateToAsync(nameof(UserProfilePage), navigationParameter);
        }
    }

    [RelayCommand]
    private async Task AddProjectAsync()
    {
        // Todo
    }
}
