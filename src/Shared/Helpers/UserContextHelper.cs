using Microsoft.AspNetCore.Http;

namespace Baustellen.App.Shared.Helpers;

public static class UserContextHelper
{
    public static string GetCurrentPrincipal()
    {
        try
        {
            var httpContext = new HttpContextAccessor().HttpContext;
            if (httpContext != null && httpContext.User.Identity?.Name != null)
            {
                var principalMame = httpContext.User.Identity.Name;

                if (principalMame != null)
                {
                    return principalMame;
                }
            }
        }
        catch (Exception)
        {
            return string.Empty;
        }
        return string.Empty;
    }
}
