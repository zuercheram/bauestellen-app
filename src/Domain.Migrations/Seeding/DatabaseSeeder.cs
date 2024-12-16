using Baustellen.App.Domain.Entities;

namespace Baustellen.App.Domain.Migrations.Seeding
{
    internal class DatabaseSeeder()
    {
        internal static async Task SeedProjectesAsync(BaustellenAppDbContext dbContext, CancellationToken stoppingToken)
        {
            for (int i = 0; i < 5; i++)
            {
                var datetime = DateTime.UtcNow;

                dbContext.Projects.Add(CreateProject($"Project {i++}", $"Diese Projekt ist ein generischer, zu testzwecken eingefügter Datensatz.", datetime.AddMonths(i - 3), datetime.AddMonths(i + 2)));
            }

            await dbContext.SystemSaveChangesAsync(stoppingToken);
        }

        private static Project CreateProject(string name, string description, DateTime start, DateTime end)
        {
            return new Project
            {
                Name = name,
                Description = description,
                StartDate = start,
                EndDate = end,
            };
        }
    }
}
