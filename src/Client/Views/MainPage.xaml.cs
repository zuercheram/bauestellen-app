using Baustellen.App.Client.ViewModels;

namespace Baustellen.App.Client.Views;

public partial class MainPage
{
    public MainPage(MainPageViewModel viewModel)
    {
        BindingContext = viewModel;
        InitializeComponent();
    }

    private void ContentPageBase_NavigatedTo(object sender, NavigatedToEventArgs e)
    {
        offlineProjectCollection.SelectedItem = null;
        onlineProjectCollection.SelectedItem = null;
        Task.Run(() => ((MainPageViewModel)BindingContext).ReloadAsync());
    }
}

