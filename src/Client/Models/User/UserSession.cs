namespace Baustellen.App.Client.Models.User;

public class UserSession
{
    public bool IsAuthenticated { get; set; }
    public string UserName { get; set; } = string.Empty;
}
