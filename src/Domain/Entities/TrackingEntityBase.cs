namespace Baustellen.App.Domain.Entities;

public class TrackingEntityBase
{
    public int CreatedByOid { get; set; }
    public DateTime CreatedAt { get; set; }
    public int ModifiedByOid { get; set; }
    public DateTime ModifiedAt { get; set; }
}