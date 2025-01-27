namespace Baustellen.App.Shared.Models.Base;

public class TrackingEntityBase
{
    public int CreatedByOid { get; set; }
    public DateTime CreatedAt { get; set; }
    public int ModifiedByOid { get; set; }
    public DateTime ModifiedAt { get; set; }
}