using Baustellen.App.Client.Models;

namespace Baustellen.App.Client.ViewModels;

public class ConnectivityViewModel : ViewModelBase
{
    private readonly ConnectivityModel _connectivityModel;

    public ConnectivityViewModel(ConnectivityModel model)
    {
        _connectivityModel = model;
        _connectivityModel.PropertyChanged += _connectivityModel_PropertyChanged;
    }

    ~ConnectivityViewModel()
    {
        _connectivityModel.PropertyChanged -= _connectivityModel_PropertyChanged;
    }

    private void _connectivityModel_PropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
    {
        OnPropertyChanged(nameof(IsOnline));
        OnPropertyChanged(nameof(IsOffline));
        OnPropertyChanged(nameof(IsUploading));
        OnPropertyChanged(nameof(IsDownloading));
    }

    public bool IsOnline { get => _connectivityModel.IsOnline; }

    public bool IsOffline { get => !_connectivityModel.IsOnline; }

    public bool IsUploading { get => _connectivityModel.IsUploading; }
    public bool IsDownloading { get => _connectivityModel.IsDownloading; }
}
