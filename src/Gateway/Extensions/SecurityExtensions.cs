using Baustellen.App.ServiceDefaults;
using Baustellen.App.Shared.Constants;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;

namespace Baustellen.App.Gateway.Extensions;

public static class SecurityExtensions
{
    public static void AddApplicationSecurity(this IHostApplicationBuilder builder)
    {
        var configuration = builder.Configuration;
        var services = builder.Services;

        JsonWebTokenHandler.DefaultInboundClaimTypeMap.Remove("sub");
                
        var sessionCookieLifetime = configuration.GetValue("SessionCookieLifetimeMinutes", 60);

        services.AddAuthorization(options =>
        {
            options.AddPolicy("is-user", policy => policy.RequireAuthenticatedUser());
            options.AddPolicy("is-manager", policy => policy.RequireAuthenticatedUser());
        });

        services.AddAuthentication(options => 
        {
            options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = OpenIdConnectDefaults.AuthenticationScheme;
        })
        .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme, options => options.ExpireTimeSpan = TimeSpan.FromMinutes(sessionCookieLifetime))
        .AddKeycloakOpenIdConnect(serviceName: AppConstants.IdentityApiProject, realm: "baustellen-app", options =>
        {
            options.ClientId = "BaustellenApp";
            options.ResponseType = OpenIdConnectResponseType.Code;
            options.Scope.Add("projects:all");
            options.SaveTokens = true;
            options.TokenValidationParameters.NameClaimType = JwtRegisteredClaimNames.Name;
            options.SignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
            options.RequireHttpsMetadata = false;
        });
    }
}
