using Baustellen.App.Projects.Api.Models;
using Baustellen.App.Shared.Data.Base;
using Baustellen.App.Shared.Extensions;
using Microsoft.EntityFrameworkCore;

namespace Baustellen.App.Projects.Api.Data;

public class ProjectsDbContext : TrackingDbContext<ProjectsDbContext>
{
    public ProjectsDbContext(DbContextOptions<ProjectsDbContext> options)
        : base(options)
    {
    }

    public DbSet<Project> Projects { get; set; }
    public DbSet<ExternalLinks> ExternalLinks { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Project>().IsTrackingEntity();

        base.OnModelCreating(modelBuilder);
    }
}