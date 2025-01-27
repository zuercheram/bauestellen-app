using Microsoft.Extensions.DependencyInjection;

namespace Baustellen.App.Service.Extensions;

public static class ServiceCollectionExtensions 
{
    public static void AddServices(this IServiceCollection services)
    {
        services.AddTransient<ProjectService>();
    }
}