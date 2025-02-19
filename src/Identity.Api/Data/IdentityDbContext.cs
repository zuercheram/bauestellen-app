using Baustellen.App.Identity.Api.Models;
using Baustellen.App.Shared.Data.Base;
using Baustellen.App.Shared.Extensions;
using Microsoft.EntityFrameworkCore;

namespace Baustellen.App.Identity.Api.Data;

public class IdentityDbContext : TrackingDbContext<IdentityDbContext>
{
    public IdentityDbContext(DbContextOptions<IdentityDbContext> options) : base(options) { }

    public DbSet<User> Users { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>().IsTrackingEntity();

        base.OnModelCreating(modelBuilder);
    }
}
