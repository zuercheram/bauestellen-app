using Baustellen.App.Projects.Api.Models;
using Baustellen.App.Shared.Extensions;
using Baustellen.App.Shared.Models.Base;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Baustellen.App.Projects.Api.Data;

public class ProjectsDbContext : DbContext
{
    public ProjectsDbContext(DbContextOptions<ProjectsDbContext> options)
        : base(options)
    {
    }

    public DbSet<Project> Projects { get; set; }
    public DbSet<Customer> Customers { get; set; }
    public DbSet<ExternalLinks> ExternalLinks { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Project>().IsTrackingEntity();
        modelBuilder.Entity<Customer>().IsTrackingEntity();

        base.OnModelCreating(modelBuilder);
    }

    public override int SaveChanges()
    {
        UpdateTrackingEntityProperties();
        return base.SaveChanges();
    }

    public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken)
    {
        UpdateTrackingEntityProperties();
        return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
    }

    public Task<int> SystemSaveChangesAsync(CancellationToken cancellationToken)
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