using Baustellen.App.Client.ViewModels;

namespace Baustellen.App.Client.Views;

public partial class EditProjectPage
{
	public EditProjectPage(EditProjectViewModel viewModel)
	{
		BindingContext = viewModel;
		InitializeComponent();
	}
}