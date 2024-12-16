using Baustellen.App.Client.Services.ProjectService;

namespace Baustellen.App.Client.Services.AppEnvironment;

internal class AppEnvironmentService(IProjectService projectService, IProjectService mockProjectService) : IAppEnvironmentService
{
    public IProjectService ProjectService { get; private set; }

    public void UpdateDependencies(bool useMockSerivces)
    {
        if (useMockSerivces)
        {
            ProjectService = mockProjectService;
        }
        else
        {
            ProjectService = projectService;
        }
    }
}
