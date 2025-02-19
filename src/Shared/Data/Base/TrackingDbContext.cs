using Baustellen.App.Shared.Models.Base;
using Microsoft.EntityFrameworkCore;

namespace Baustellen.App.Shared.Data.Base;

public class TrackingDbContext<T>: DbContext where T : DbContext
{
    public TrackingDbContext(DbContextOptions<T> options) : base(options) {}

    public override int SaveChanges()
    {
        UpdateTrackingEntityProperties();
        return base.SaveChanges();
    }

    public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default)
    {
        UpdateTrackingEntityProperties();
        return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
    }

    public Task<int> SystemSaveChangesAsync(CancellationToken cancellationToken = default)
    {
        UpdateTrackingEntityProperties(true);
        return base.SaveChangesAsync(true, cancellationToken);
    }

    private void UpdateTrackingEntityProperties(bool isSystemUser = false)
    {
        var userId = isSystemUser ? 0 : 0; // TODO should be a real user oid in the future.

        var now = DateTime.UtcNow;
        var addedEntities = ChangeTracker.Entries().Where(x => x.State == EntityState.Added && x.Entity is TrackingEntityBase).Select(x => (TrackingEntityBase)x.Entity);

        foreach (var entity in addedEntities)
        {
            entity.CreatedAt = entity.ModifiedAt = now;
            entity.CreatedByOid = entity.ModifiedByOid = userId;
        }

        var modifiedEntities = ChangeTracker.Entries().Where(x => x.State == EntityState.Modified && x.Entity is TrackingEntityBase).Select(x => (TrackingEntityBase)x.Entity);

        foreach (var entity in modifiedEntities)
        {
            entity.ModifiedAt = now;
            entity.ModifiedByOid = userId;
        }
    }
}
