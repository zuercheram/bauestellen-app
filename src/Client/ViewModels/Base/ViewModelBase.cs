using Baustellen.App.Client.Services.Navigation;

namespace Baustellen.App.Client.ViewModels.Base;

public abstract partial class ViewModelBase : ObservableObject, IViewModelBase
{
    private long _isBusy;

    [ObservableProperty]
    private bool _isInitialized;

    public ViewModelBase(INavigationService navigationService)
    {
        NavigationService = navigationService;

        InitializeAsyncCommand = new AsyncRelayCommand(async () =>
        {
            await IsBusyFor(InitializeAsync);
            IsInitialized = true;
        },
        AsyncRelayCommandOptions.FlowExceptionsToTaskScheduler);
    }

    public INavigationService NavigationService { get; }

    public IAsyncRelayCommand InitializeAsyncCommand { get; }

    public bool IsBusy => Interlocked.Read(ref _isBusy) > 0;

    public virtual void ApplyQueryAttributes(IDictionary<string, object> query)
    {
    }

    public virtual Task InitializeAsync()
    {
        return Task.CompletedTask;
    }

    protected async Task IsBusyFor(Func<Task> unitOfWork)
    {
        Interlocked.Increment(ref _isBusy);
        OnPropertyChanged(nameof(IsBusy));

        try
        {
            await unitOfWork();
        }
        finally
        {
            Interlocked.Decrement(ref _isBusy);
            OnPropertyChanged(nameof(IsBusy));
        }
    }
}
