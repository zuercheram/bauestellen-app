using Baustellen.App.Client.Models;
using Baustellen.App.Client.Services;

namespace Baustellen.App.Client.ViewModels;

public class ConnectivityViewModel : ViewModelBase
{
    private readonly SyncingService _syncingService;

    private bool _isSyncing;

    public bool IsSyncing
    {
        get => _isSyncing;
        set => SetProperty(ref _isSyncing, value);
    }

    public bool IsBusy => ConnectivityModel.IsBusy;

    public ConnectivityViewModel(ConnectivityModel model, SyncingService syncingService) : base(model)
    {
        _syncingService = syncingService;

        ConnectivityModel.PropertyChanged += ConnectivityModel_PropertyChanged;

        _syncingService.ProjectSyncing += SyncingService_ProjectSyncing;
        _syncingService.ProjectSynced += SyncingService_ProjectSynced;
        _syncingService.UserSyncing += SyncingService_UserSyncing;
        _syncingService.UserSynced += SyncingService_UserSynced;
    }

    private void SyncingService_UserSynced(object? sender, EventArgs e)
    {
        IsSyncing = false;
    }

    private void SyncingService_UserSyncing(object? sender, EventArgs e)
    {
        IsSyncing = true;
    }

    private void SyncingService_ProjectSynced(object? sender, EventArgs e)
    {
        IsSyncing = false;
    }

    private void SyncingService_ProjectSyncing(object? sender, EventArgs e)
    {
        IsSyncing = true;
    }

    ~ConnectivityViewModel()
    {
        ConnectivityModel.PropertyChanged -= ConnectivityModel_PropertyChanged;

        _syncingService.ProjectSyncing -= SyncingService_ProjectSyncing;
        _syncingService.ProjectSynced -= SyncingService_ProjectSynced;
        _syncingService.UserSyncing -= SyncingService_UserSyncing;
        _syncingService.UserSynced -= SyncingService_UserSynced;
    }

    private void ConnectivityModel_PropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
    {
        OnPropertyChanged(nameof(IsBusy));
        OnPropertyChanged(nameof(IsOnline));
        OnPropertyChanged(nameof(IsOffline));
    }

    public bool IsOnline { get => ConnectivityModel.IsOnline; }

    public bool IsOffline { get => !ConnectivityModel.IsOnline; }
}
