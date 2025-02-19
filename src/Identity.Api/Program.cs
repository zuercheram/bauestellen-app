using Baustellen.App.Identity.Api.Data;
using Baustellen.App.Identity.Api.Data.Seeding;
using Baustellen.App.Identity.Api.Services;
using Baustellen.App.ServiceDefaults;
using Baustellen.App.Shared.Constants;
using Baustellen.App.Shared.Extensions;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Identity.Web;

var builder = WebApplication.CreateBuilder(args);

builder.AddNpgsqlDbContext<IdentityDbContext>(AppConstants.PostgresIdentityDatabaseName);
builder.Services.AddMigration<IdentityDbContext, IdentityDbSeeding>();
builder.Services.AddTransient<UserService>();
builder.AddServiceDefaults();

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddMicrosoftIdentityWebApi(builder.Configuration, "AzureAd");
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("UsersRead", policy =>
    {
        policy.RequireScope("Users.Read");
    });
});
// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();

app.UseAuthentication();
app.UseAuthorization();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.MapGet("api/users", async () =>
{
    using var scope = app.Services.CreateScope();    
    var service = scope.ServiceProvider.GetRequiredService<UserService>();
    return await service.GetUsersAsync();
})
.RequireAuthorization("UsersRead")
.WithName("GetUsers");

app.MapGet("api/user/id/{id}", async (int id) =>
{
    using var scope = app.Services.CreateScope();
    var service = scope.ServiceProvider.GetRequiredService<UserService>();
    return await service.GetUserByAsync(id);
})
.RequireAuthorization("UsersRead")
.WithName("GetUserById");

app.MapGet("api/user/email/{email}", async (string email) =>
{
    using var scope = app.Services.CreateScope();
    var service = scope.ServiceProvider.GetRequiredService<UserService>();
    return await service.GetUserByAsync(email);
})
.RequireAuthorization("UsersRead")
.WithName("GetUserByEmail");

app.Run();