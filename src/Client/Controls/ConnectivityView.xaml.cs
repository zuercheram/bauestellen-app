using Baustellen.App.Client.ViewModels;

namespace Baustellen.App.Client.Controls;

public partial class ConnectivityView : ContentView
{
    public ConnectivityView()
    {
        InitializeComponent();

        HandlerChanged += OnHandlerChanged;
    }

    void OnHandlerChanged(object sender, EventArgs e)
    {
        BindingContext = Handler.MauiContext.Services.GetRequiredService<ConnectivityViewModel>();
    }
}