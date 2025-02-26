namespace Baustellen.App.Client.Authentication.MSALClient;

public class DownStreamApiHelper
{
    private string[] DownstreamApiScopes;
    public DownStreamApiConfig DownstreamApiConfig;
    private MSALClientHelper MSALClient;

    public DownStreamApiHelper(DownStreamApiConfig downstreamApiConfig, MSALClientHelper msalClientHelper)
    {
        if (msalClientHelper == null)
        {
            throw new ArgumentNullException(nameof(msalClientHelper));
        }

        this.DownstreamApiConfig = downstreamApiConfig;
        this.MSALClient = msalClientHelper;
        this.DownstreamApiScopes = this.DownstreamApiConfig.ScopesArray;
    }
}
