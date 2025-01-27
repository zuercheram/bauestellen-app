using Baustellen.App.Client.Services.Navigation;

namespace Baustellen.App.Client.ViewModels.Base;

public interface IViewModelBase : IQueryAttributable
{

    public INavigationService NavigationService { get; }

    public IAsyncRelayCommand InitializeAsyncCommand { get; }

    public bool IsBusy { get; }

    public bool IsInitialized { get; }

    Task InitializeAsync();
}
