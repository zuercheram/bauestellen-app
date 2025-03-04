using Baustellen.App.Client.Data.Entities;
using NodaTime;

namespace Baustellen.App.Client.Data.Repositories;

public class ProjectRepository : RepositoryBase
{
    public ProjectRepository(string dbPath) : base(dbPath)
    {
    }

    public override void Init()
    {
        base.Init();

        conn.CreateTable<Project>();
    }

    public void AddProject(Project project)
    {
        int result = 0;

        try
        {
            Init();

            project.ModifiedAt = DateTime.UtcNow;

            if (project.Id == Guid.Empty)
            {
                project.Id = Guid.NewGuid();
                result = conn.Insert(project);
            }
            else
            {
                result = conn.Update(project);
            }

            StatusMessage = string.Format("{0} record(s) added.", result);
        }
        catch (Exception ex)
        {
            StatusMessage = string.Format("Failed to add {0}. Error: {1}", nameof(project), ex.Message);
        }
    }

    public void DeleteProject(Project project)
    {
        if (project.Id == Guid.Empty)
        {
            throw new ArgumentNullException(nameof(project.Id));
        }

        int result = 0;

        try
        {
            Init();

            result = conn.Delete(project);

            StatusMessage = string.Format("{0} record(s) deleted.", result);
        }
        catch (Exception ex)
        {
            StatusMessage = string.Format("Failed to delete {0}. Error: {1}", nameof(project), ex.Message);
        }
    }

    public List<Project> GetAll()
    {
        try
        {
            Init();
            return conn.Table<Project>().ToList();
        }
        catch (Exception ex)
        {
            StatusMessage = string.Format("Failed to retrieve data. {0}", ex.Message);
        }

        return new List<Project>();
    }

    public Project? Get(Guid id)
    {
        try
        {
            Init();
            return conn.Table<Project>().FirstOrDefault(x => x.Id == id);
        }
        catch (Exception ex)
        {
            StatusMessage = string.Format("Failed to retrieve data. {0}", ex.Message);
        }

        return null;
    }
}
