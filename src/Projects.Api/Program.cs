using Baustellen.App.Projects.Api.Data;
using Baustellen.App.Projects.Api.Data.Seeding;
using Baustellen.App.Projects.Api.Extensions;
using Baustellen.App.ServiceDefaults;
using Baustellen.App.Shared.Constants;
using Baustellen.App.Shared.Extensions;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Identity.Web;

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
builder.AddNpgsqlDbContext<ProjectsDbContext>(AppConstants.PostgresProjectDatabaseName);
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
