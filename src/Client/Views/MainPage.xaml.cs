using Baustellen.App.Client.ViewModels;

namespace Baustellen.App.Client.Views;

public partial class MainPage
{
    public MainPage(MainPageViewModel viewModel)
    {
        BindingContext = viewModel;
        InitializeComponent();
    }
}

