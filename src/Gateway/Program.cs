using Baustellen.App.Gateway.Extensions;
using Baustellen.App.ServiceDefaults;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

builder.Services.AddReverseProxy().LoadFromConfig(builder.Configuration.GetSection("ReverseProxy")).AddServiceDiscoveryDestinationResolver();
builder.Services.AddHttpForwarderWithServiceDiscovery();

builder.AddApplicationSecurity();

var app = builder.Build();

app.UseAuthentication();
app.UseAuthorization();

app.MapDefaultEndpoints();
app.MapReverseProxy();

await app.RunAsync();
