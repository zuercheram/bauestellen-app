using Baustellen.App.Projects.Api.Services;

namespace Baustellen.App.Projects.Api.Extensions;

public static class ServiceCollectionExtensions
{
    public static void AddServices(this IServiceCollection services)
    {
        services.AddTransient<ProjectService>();
    }
}