using Baustellen.App.AppHost.Extensions;
using Baustellen.App.Shared.Constants;
using Projects;

var builder = DistributedApplication.CreateBuilder(args);

builder.AddForwardedHeaders();

var postgres = builder.AddAzurePostgresFlexibleServer(AppConstants.PostgresServerName);

var projectDb = postgres.AddDatabase(AppConstants.PostgresProjectDatabaseName);
var identityDb = postgres.AddDatabase(AppConstants.PostgresIdentityDatabaseName);

var launchProfileName = "https";

var identityApi = builder.AddProject<Baustellen_App_Identity_Api>(AppConstants.IdentityApi, launchProfileName)
    .WaitFor(identityDb)
    .WithReference(identityDb);

var projectApi = builder.AddProject<Baustellen_App_Projects_Api>(AppConstants.ProjectApi, launchProfileName)
    .WaitFor(projectDb)
    .WithReference(projectDb)
    .WithReference(identityApi);

var gateway = builder.AddProject<Baustellen_App_Gateway>(AppConstants.GatewayApi)
    .WithReference(projectApi)
    .WithReference(identityApi)
    .WithExternalHttpEndpoints();

builder.Build().Run();
