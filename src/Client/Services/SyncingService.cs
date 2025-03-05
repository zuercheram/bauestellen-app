using Baustellen.App.Client.Data.Entities;
using Baustellen.App.Client.Data.Repositories;
using Baustellen.App.Client.Models;
using Baustellen.App.Shared.Models.InputModel;

namespace Baustellen.App.Client.Services;

public  class SyncingService
{
    private readonly AppUserRepository _userRepository;
    private readonly ProjectRepository _projectRepository;
    private readonly ProjectService _projectService;
    private readonly UserService _userService;
    private readonly ConnectivityModel _connectivityModel;
    private readonly AuthUserModel _authUserModel;

    private bool _projectIsSyncing = false;
    private bool _userIsSyncing = false;
    private bool _timerStarted = false;

    private System.Timers.Timer _timer;

    public SyncingService(
        AppUserRepository appUserRepository,
        ProjectRepository projectRepository,
        ProjectService projectService,
        UserService userService,
        ConnectivityModel connectivityModel,
        AuthUserModel authUserModel
        )
    {
        _userRepository = appUserRepository;
        _projectRepository = projectRepository;
        _projectService = projectService;
        _userService = userService;
        _connectivityModel = connectivityModel;
        _authUserModel = authUserModel;
    }

    public event EventHandler ProjectSyncing;
    public event EventHandler ProjectSynced;
    public event EventHandler UserSyncing;
    public event EventHandler UserSynced;

    public void Start()
    {
        if (_timerStarted)
        {
            return;
        }
        _timer = new System.Timers.Timer(3600000);
        _timer.Elapsed += async (sender, e) =>
        {
            await SyncProjects();
            await SyncAppUser();
        };
        _timer.Start();
        _timerStarted = true;
    }

    public void Stop()
    {
        _timer.Stop();
        _timer.Dispose();
        _timerStarted = false;
    }

    public async Task SyncProjects()
    {
        if (!_authUserModel.IsLoggedIn || !_connectivityModel.IsOnline || _projectIsSyncing)
        {
            return;
        }

        OnProjectSyncing(new EventArgs());
        try
        {
            var projects = _projectRepository.GetAll();
            if (projects.Count == 0)
            {
                return;
            }

            var syncTimestamps = projects.Select(x => new KeyValuePair<Guid, DateTime>(x.Id, x.ModifiedAt)).ToDictionary() ?? [];
            var syncInputRequest = new ProjectSyncInputDto
            {
                SyncIdTimestamps = syncTimestamps!,
            };

            var response = await _projectService.RequestProjectSynchroAsync(syncInputRequest);
            foreach (var projectView in response.NewProjects)
            {
                _projectRepository.AddProject(Project.CopyToProject(projectView));
            }

            var projectsToUpdate = new Dictionary<Guid, ProjectInputDto>();
            foreach (var outdatedId in response.OutdatedProjects)
            {
                var project = _projectRepository.Get(outdatedId);
                if (project == null)
                {
                    continue;
                }
                projectsToUpdate.Add(project.Id, Project.CopyToInputDto(project));
            }

            if (projectsToUpdate.Count == 0)
            {
                return;
            }

            var updateRequest = new ProjectUpdateInputDto
            {
                UpdateProjects = projectsToUpdate
            };
            await _projectService.UpdateProjectsAsync(updateRequest);
        }
        catch (Exception) { }
        finally
        {
            OnProjectSynced(new EventArgs());
        }
    }

    public async Task SyncAppUser()
    {
        if (!_authUserModel.IsLoggedIn || !_connectivityModel.IsOnline || _userIsSyncing)
        {
            return;
        }

        try
        {
            OnUserSyncing(new EventArgs());
            var lastModified = _userRepository.GetLastModifiedTicks();
            var userUpdated = await _userService.SyncUserAsync(lastModified);
            _userRepository.DeleteAll();
            foreach (var user in userUpdated)
            {
                _userRepository.AddAppUser(AppUser.CopyToAppUser(user));
            }
        }
        catch (Exception) { }
        finally
        {
            OnUserSynced(new EventArgs());
        }
    }

    protected virtual void OnProjectSyncing(EventArgs e)
    {
        _projectIsSyncing = true;
        ProjectSyncing?.Invoke(this, e);
    }

    protected virtual void OnProjectSynced(EventArgs e)
    {
        _projectIsSyncing = false;
        ProjectSynced?.Invoke(this, e);
    }

    protected virtual void OnUserSyncing(EventArgs e)
    {
        _userIsSyncing = true;
        UserSyncing?.Invoke(this, e);
    }

    protected virtual void OnUserSynced(EventArgs e)
    {
        _userIsSyncing = false;
        UserSynced?.Invoke(this, e);
    }
}
