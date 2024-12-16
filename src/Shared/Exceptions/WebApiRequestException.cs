namespace Baustellen.App.Shared.Exceptions;
public class WebApiRequestException : Exception
{
    // TODO Implement some crazy shit to handle errors from web api calls
    public WebApiRequestException(string message) { }

    public WebApiRequestException(Exception ex) { }

    public WebApiRequestException(Exception ex, string message) { }
}
