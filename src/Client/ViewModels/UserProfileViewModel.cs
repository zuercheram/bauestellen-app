using Baustellen.App.Client.Extensions;
using Baustellen.App.Client.Models;
using Baustellen.App.Client.Views;
using Baustellen.App.Shared.Constants;
using Baustellen.App.Client.Helper;

namespace Baustellen.App.Client.ViewModels;

public partial class UserProfileViewModel : ViewModelBase
{
    public string AppTitle => AppInfo.Name;
    public string Version => AppInfo.VersionString;
    public string MoreInfoUrl => "https://aka.ms/maui";
    public string Message => "This app is written in XAML and C# with .NET MAUI.";

    private readonly AuthUserModel _userModel;
    private bool _initialized;

    public string FirstName
    {
        get { return _userModel.AuthenticatedUser?.FirstName ?? string.Empty; }
    }

    public string LastName
    {
        get { return _userModel.AuthenticatedUser?.LastName ?? string.Empty; }
    }

    public string Email
    {
        get { return _userModel.AuthenticatedUser?.Email ?? string.Empty; }
    }

    public string UserRole
    {
        get { return _userModel.AuthenticatedUser?.Role.ToString() ?? string.Empty; }
    }

    public string Title
    {
        get => $"{FirstName} {LastName}";
    }

    public bool IsLoggedIn
    {
        get { return _userModel.IsLoggedIn; }
        set
        {
            SetProperty<bool>(_userModel.IsLoggedIn, value, (newValue) =>
            {
                _userModel.IsLoggedIn = newValue;
            });
        }
    }

    private bool IsRoutedToForSignIn { get; set; }

    public UserProfileViewModel(AuthUserModel userModel)
    {
        _userModel = userModel;
    }

    public override async Task InitializeAsync()
    {
        if (_userModel.IsLoggedIn)
        {
            return;
        }

        await IsBusyFor(
            async () =>
            {
                if (IsRoutedToForSignIn)
                {
                    await _userModel.SignIn();
                    RefreshProperties();
                    await Navigation.NavigateToAsync($"///{nameof(MainPage)}");
                }
            });
    }

    public override void ApplyQueryAttributes(IDictionary<string, object> query)
    {
        base.ApplyQueryAttributes(query);

        if (query.ContainsKey(AppConstants.ClientAutoSignInRoute))
        {
            IsRoutedToForSignIn = query.ValueAsBool(AppConstants.ClientAutoSignInRoute);
        }
    }

    private void RefreshProperties()
    {
        OnPropertyChanged(nameof(IsLoggedIn));
        OnPropertyChanged(nameof(LastName));
        OnPropertyChanged(nameof(FirstName));
        OnPropertyChanged(nameof(Email));
        OnPropertyChanged(nameof(UserRole));
    }

    [RelayCommand]
    private async Task SignIn()
    {
        await IsBusyFor(async () =>
        {
            await _userModel.SignIn();
            RefreshProperties();
        });
    }

    [RelayCommand]
    private async Task SignOut()
    {
        await IsBusyFor(async () =>
        {
            await _userModel.SignOut();
            RefreshProperties();
        });
    }
}
