using NodaTime;

namespace Baustellen.App.Shared.Models.Base;

public class TrackingEntityBase
{
    public string CreatedByOid { get; set; }
    public DateTime CreatedAt { get; set; }
    public string ModifiedByOid { get; set; }
    public DateTime ModifiedAt { get; set; }
}