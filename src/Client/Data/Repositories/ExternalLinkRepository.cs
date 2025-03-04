using Baustellen.App.Client.Data.Entities;
using Baustellen.App.Shared.Models;

namespace Baustellen.App.Client.Data.Repositories;

public class ExternalLinkRepository : RepositoryBase
{
    public ExternalLinkRepository(string dbPath) : base(dbPath)
    {
    }

    public override void Init()
    {
        base.Init();

        conn.CreateTable<ExternalLink>();
    }

    public void AddExternalLink(Guid linkId, string link, Guid projectId, LinkTypeEnum linkType)
    {
        if (projectId == Guid.Empty)
        {
            throw new ArgumentNullException(nameof(projectId));
        }

        int result = 0;

        try
        {
            Init();

            if (string.IsNullOrEmpty(link))
                throw new Exception("Valid link required");

            var uri = new Uri(link);

            if (linkId == Guid.Empty)
            {
                result = conn.Insert(new ExternalLink
                {
                    Id = Guid.NewGuid(),
                    Link = uri.ToString(),
                    ProjectId = projectId,
                    Type = linkType
                });
            }
            else
            {
                result = conn.Update(new ExternalLink
                {
                    Id = linkId,
                    Link = uri.ToString(),
                    ProjectId = projectId,
                    Type = linkType
                });
            }

            StatusMessage = string.Format("{0} record(s) added (Link: {1})", result, link);
        }
        catch (ArgumentNullException ex)
        {
            StatusMessage = string.Format("Link cannot be empty or null! Error: {0}", ex.Message);
        }
        catch (UriFormatException ex)
        {
            StatusMessage = string.Format("Link must be a valid URL! Error: {0}", ex.Message);
        }
        catch (Exception ex)
        {
            StatusMessage = string.Format("Failed to add {0}. Error: {1}", link, ex.Message);
        }
    }

    public void DeleteLinks(ExternalLink link)
    {
        if (link.Id == Guid.Empty)
        {
            throw new ArgumentNullException(nameof(link.Id));
        }

        int result = 0;

        try
        {
            Init();

            result = conn.Delete(link);

            StatusMessage = string.Format("{0} record(s) deleted.", result);
        }
        catch (Exception ex)
        {
            StatusMessage = string.Format("Failed to delete {0}. Error: {1}", nameof(link), ex.Message);
        }
    }

    public List<ExternalLink> GetProjectLinks(Guid projectId)
    {
        try
        {
            Init();
            return conn.Table<ExternalLink>().Where(x => x.ProjectId == projectId).ToList();
        }
        catch (Exception ex)
        {
            StatusMessage = string.Format("Failed to retrieve data. {0}", ex.Message);
        }

        return new List<ExternalLink>();
    }
}
