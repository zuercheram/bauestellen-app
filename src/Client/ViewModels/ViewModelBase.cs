using Baustellen.App.Client.Models;

namespace Baustellen.App.Client.ViewModels;

public partial class ViewModelBase : ObservableObject, IQueryAttributable
{
    private long _isBusy;

    [ObservableProperty] private bool _isInitialized;

    public bool IsBusy => Interlocked.Read(ref _isBusy) > 0;

    public bool IsNotBusy => !IsBusy;

    public ConnectivityModel ConnectivityModel { get; set; }

    public ViewModelBase(ConnectivityModel connectivityModel)
    {
        ConnectivityModel = connectivityModel;
        InitializeAsyncCommand =
            new AsyncRelayCommand(
                async () =>
                {
                    await IsBusyFor(InitializeAsync);
                    IsInitialized = true;
                },
                AsyncRelayCommandOptions.FlowExceptionsToTaskScheduler);
    }

    public IAsyncRelayCommand InitializeAsyncCommand { get; }

    public virtual void ApplyQueryAttributes(IDictionary<string, object> query)
    {
    }

    public virtual Task InitializeAsync()
    {
        return Task.CompletedTask;
    }
    protected async Task IsBusyFor(Func<Task> unitOfWork)
    {
        ConnectivityModel.IsBusy = true;
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
            ConnectivityModel.IsBusy = false;
        }
    }
}
