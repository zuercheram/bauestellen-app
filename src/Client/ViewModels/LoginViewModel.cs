using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace Baustellen.App.Client.ViewModels;

public class LoginViewModel : INotifyPropertyChanged
{
    private string _username = string.Empty;
    public string Username
    {
        get => _username;
        set
        {
            if (_username != value)
            {
                _username = value;
                OnPropertyChanged();
            }
        }
    }

    private string _password = string.Empty;
    public string Password
    {
        get => _password;
        set
        {
            if (_password != value)
            {
                _password = value;
                OnPropertyChanged();
            }
        }
    }

    private string _currentState = string.Empty;
    public string CurrentState
    {
        get => _currentState;
        set
        {
            if (_currentState != value)
            {
                _currentState = value;
                OnPropertyChanged();
            }
        }
    }

    private bool _canStateChange = true;
    public bool CanStateChange
    {
        get => _canStateChange;
        set
        {
            if (_canStateChange != value)
            {
                _canStateChange = value;
                OnPropertyChanged();
                ((Command)LoginCommand).ChangeCanExecute();
            }
        }
    }

    public ICommand LoginCommand { get; }

    public LoginViewModel()
    {
        CurrentState = "Default";
        LoginCommand = new Command(async () => await LoginAsync(), () => CanStateChange);
    }

    private async Task LoginAsync()
    {
        if (!CanStateChange) return;

        CurrentState = "Loading";
        CanStateChange = false;

        try
        {
            await Task.Delay(2000);

            CurrentState = "Success";
            CanStateChange = true;

            await Application.Current.MainPage.DisplayAlert("Login", "Login successgul", "OK");
            CurrentState = string.Empty;
        }
        catch (Exception ex)
        {
            await Application.Current.MainPage.DisplayAlert("Error", $"Login failed: {ex.Message}", "OK");
            CurrentState = string.Empty;
            CanStateChange = true;
        }
    }

    public event PropertyChangedEventHandler? PropertyChanged;

    protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
