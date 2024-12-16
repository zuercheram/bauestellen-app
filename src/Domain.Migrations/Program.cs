using Baustellen.App.Domain;
using Baustellen.App.Domain.Migrations;
using Baustellen.App.ServiceDefaults;
using Baustellen.App.Shared.Constants;
using Microsoft.EntityFrameworkCore;

var builder = Host.CreateApplicationBuilder(args);

builder.AddServiceDefaults();
builder.Services.AddHostedService<Worker>();

builder.Services.AddOpenTelemetry()
    .WithTracing(tracing => tracing.AddSource(Worker.ActivitySourceName));

builder.AddNpgsqlDbContext<BaustellenAppDbContext>(
    AppConstants.PostgresDatabaseName,
    configureDbContextOptions: options => options.UseNpgsql(b => b.MigrationsAssembly(typeof(Worker).Assembly.FullName)));

var host = builder.Build();
host.Run();
