using Baustellen.App.Client.Data.Entities;
using Baustellen.App.Shared.Constants;
using NodaTime;

namespace Baustellen.App.Client.Data.Repositories;

public class AppUserRepository : RepositoryBase
{
    public AppUserRepository(string dbPath) : base(dbPath)
    {
    }

    public override void Init()
    {
        base.Init();

        conn.CreateTable<AppUser>();
    }

    public void AddAppUser(AppUser appUser)
    {
        int result = 0;

        try
        {
            Init();

            result = conn.Insert(appUser);

            StatusMessage = string.Format("{0} record(s) added.", result);
        }
        catch (Exception ex)
        {
            StatusMessage = string.Format("Failed to add {0}. Error: {1}", nameof(appUser), ex.Message);
        }
    }

    public void DeleteAll()
    {
        int result = 0;

        try
        {
            Init();

            result = conn.DeleteAll<AppUser>();

            StatusMessage = string.Format("{0} record(s) deleted.", result);
        }
        catch (Exception ex)
        {
            StatusMessage = string.Format("Failed to delete all {0}. Error: {1}", nameof(AppUser), ex.Message);
        }
    }

    public List<AppUser> GetProjectManagers()
    {
        try
        {
            Init();
            return conn.Table<AppUser>().Where(x => x.Role == AppRoleEnum.ProjectLead).ToList();
        }
        catch (Exception ex)
        {
            StatusMessage = string.Format("Failed to retrieve data. {0}", ex.Message);
        }

        return new List<AppUser>();
    }

    public DateTime GetLastModifiedTicks()
    {
        try
        {
            Init();
            return conn.Table<AppUser>().Max(x => x.ModifiedAt);
        }
        catch (Exception ex)
        {
            StatusMessage = string.Format("Failed to retrieve data. {0}", ex.Message);
        }

        return DateTime.MinValue;
    }
}
