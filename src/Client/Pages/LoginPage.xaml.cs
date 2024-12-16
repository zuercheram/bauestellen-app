using Baustellen.App.Client.ViewModels;

namespace Baustellen.App.Client.Pages;

public partial class LoginPage : ContentPage
{
	public LoginViewModel ViewModel { get; } = new LoginViewModel();


	public LoginPage()
	{
		InitializeComponent();
		BindingContext = ViewModel;
	}
}