using Azure.Core;
using Azure.Identity;
using Baustellen.App.Projects.Api.Data;
using Baustellen.App.Projects.Api.Data.Seeding;
using Baustellen.App.Projects.Api.Extensions;
using Baustellen.App.ServiceDefaults;
using Baustellen.App.Shared.Constants;
using Baustellen.App.Shared.Extensions;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Web;
using Npgsql;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddServices();
builder.Services.AddOpenApi();

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddMicrosoftIdentityWebApi(builder.Configuration, "AzureAd");
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("User.Read", policy =>
    {
        policy.RequireScope("User.Read");
    });
});

// Add database
var datasourceBuilder = new NpgsqlDataSourceBuilder(builder.Configuration.GetConnectionString(AppConstants.PostgresProjectDatabaseName));
if (string.IsNullOrEmpty(datasourceBuilder.ConnectionStringBuilder.Password))
{
    datasourceBuilder.UsePeriodicPasswordProvider(async (_, ct) =>
        {
            var credentials = new DefaultAzureCredential();
            var token = await credentials.GetTokenAsync(
                new TokenRequestContext([
                    "https://ossrdbms-aad.database.windows.net/.default"
                ]), ct);

            return token.Token;
        },
        TimeSpan.FromHours(24),
        TimeSpan.FromSeconds(10)
    );
}

builder.Services.AddDbContext<ProjectsDbContext>(options =>
{
    options.UseNpgsql(datasourceBuilder.Build());
});

builder.Services.AddMigration<ProjectsDbContext, ProjectDbSeeding>();
builder.AddServiceDefaults();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();
