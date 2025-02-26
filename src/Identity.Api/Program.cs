using Baustellen.App.Identity.Api.Data;
using Baustellen.App.Identity.Api.Data.Seeding;
using Baustellen.App.Identity.Api.Services;
using Baustellen.App.ServiceDefaults;
using Baustellen.App.Shared.Constants;
using Baustellen.App.Shared.Extensions;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Web;
using System.Security.Claims;

var builder = WebApplication.CreateBuilder(args);

builder.AddNpgsqlDbContext<IdentityDbContext>(AppConstants.PostgresIdentityDatabaseName);
builder.Services.AddMigration<IdentityDbContext, IdentityDbSeeding>();
builder.Services.AddTransient<UserService>();
builder.AddServiceDefaults();

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddMicrosoftIdentityWebApi(builder.Configuration, "AzureAd");
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("User.Read", policy =>
    {
        policy.RequireScope("User.Read");
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

app.MapGet("api/users/{timestamp}", async(long timestamp, [FromServices] UserService service) =>
{
    return await service.GetUsersAsync(timestamp);
})
.RequireAuthorization("User.Read")
.WithName("GetUsers");

app.MapGet("api/user/id/{id}", async (int id, [FromServices] UserService service) =>
{
    return await service.GetUserByAsync(id);
})
.RequireAuthorization("User.Read")
.WithName("GetUserById");

app.MapGet("api/user/auth", async (HttpContext context, [FromServices] UserService service) =>
{
    var principalName = context.User.FindFirst(ClaimTypes.Name)!.Value;
    return await service.GetUserByPrincipalAsync(principalName);
})
.RequireAuthorization("User.Read")
.WithName("GetAuthenticatedUser");

app.Run();