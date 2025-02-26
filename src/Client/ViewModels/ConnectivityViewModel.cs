namespace Baustellen.App.Client.ViewModels;

public class ConnectivityViewModel : ViewModelBase
{
    public string NetworkAccess => Connectivity.NetworkAccess.ToString();

    public string ConnectionProfiles
    {
        get
        {
            var profiles = string.Empty;
            foreach (var p in Connectivity.ConnectionProfiles)
                profiles += "\n" + p.ToString();
            return profiles;
        }
    }

    public void OnAppearing()
    {
        Connectivity.ConnectivityChanged += OnConnectivityChanged;
    }

    public void OnDisappearing()
    {
        Connectivity.ConnectivityChanged -= OnConnectivityChanged;
    }

    void OnConnectivityChanged(object sender, ConnectivityChangedEventArgs e)
    {
        OnPropertyChanged(nameof(ConnectionProfiles));
        OnPropertyChanged(nameof(NetworkAccess));
    }
}
