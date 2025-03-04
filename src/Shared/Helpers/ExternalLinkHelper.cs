using Baustellen.App.Shared.Models;

namespace Baustellen.App.Shared.Helpers;

public class ExternalLinkHelper
{
    public static LinkTypeEnum GetLinkType(string externalLinks)
    {
        var url = new Uri(externalLinks);
        if (url.Host.Contains("teams.microsoft.com"))
        {
            return LinkTypeEnum.MsTeams;
        }
        else if (url.Host.Contains("sharepoint.com"))
        {
            return LinkTypeEnum.MsSharepoint;
        }
        else
        {
            return LinkTypeEnum.Website;
        }
    }
}
