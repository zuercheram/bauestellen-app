using Baustellen.App.Client.ViewModels;

namespace Baustellen.App.Client.Views;

public partial class UserProfilePage
{
	public UserProfilePage(UserProfileViewModel viewModel)
	{
		BindingContext = viewModel;
		InitializeComponent();
	}
}