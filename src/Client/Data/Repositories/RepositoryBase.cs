using SQLite;

namespace Baustellen.App.Client.Data.Repositories;

public class RepositoryBase(string dbPath)
{
    string _dbPath = dbPath;
    public SQLiteConnection conn;

    public string StatusMessage { get; set; } = string.Empty;

    public virtual void Init()
    {
        if (conn != null)
        {
            return;
        }

        conn = new SQLiteConnection(_dbPath);
    }
}
