using Baustellen.App.Client.Services.ProjectService;

namespace Baustellen.App.Client.Services.AppEnvironment;

internal interface IAppEnvironmentService
{
    IProjectService ProjectService { get; }
    void UpdateDependencies(bool useMockServices);
}
