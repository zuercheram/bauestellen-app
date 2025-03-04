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
        if (Handler?.MauiContext != null)
        {
            BindingContext = Handler.MauiContext.Services.GetRequiredService<ConnectivityViewModel>();
        }
    }
}