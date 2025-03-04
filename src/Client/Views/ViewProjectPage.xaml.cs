using Baustellen.App.Client.ViewModels;

namespace Baustellen.App.Client.Views;

public partial class ViewProjectPage
{
	public ViewProjectPage(ViewProjectViewModel viewModel)
	{
		BindingContext = viewModel;
		InitializeComponent();
	}
}