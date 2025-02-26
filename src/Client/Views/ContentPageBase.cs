using Baustellen.App.Client.ViewModels;

namespace Baustellen.App.Client.Views;

public class ContentPageBase : ContentPage
{
    public ContentPageBase()
    {
        NavigationPage.SetBackButtonTitle(this, string.Empty);
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        if (BindingContext is not ViewModelBase vmb)
        {
            return;
        }

        await vmb.InitializeAsyncCommand.ExecuteAsync(null);
    }
}
