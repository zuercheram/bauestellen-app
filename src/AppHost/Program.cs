using Projects;
using Baustellen.App.Shared.Constants;

var builder = DistributedApplication.CreateBuilder(args);

var postgres = builder.AddPostgres(AppConstants.PostgresServerName)
    .WithPgAdmin()
    .WithDataVolume(AppConstants.PostgresDataVolumeName, isReadOnly: false);    
var postgresDb = postgres.AddDatabase(AppConstants.PostgresDatabaseName);

builder.AddProject<Baustellen_App_Domain_Migrations>(AppConstants.AppMigrationsProject)
    .WithReference(postgresDb);

var apiService = builder.AddProject<Baustellen_App_Api>(AppConstants.AppApiProject)
    .WithReference(postgresDb)
    .WithExternalHttpEndpoints();

builder.AddMobileProject(AppConstants.MobileAppProject, "../Client", clientStubProjectPath: "../ClientStub/Baustellen.App.ClientStub.csproj")
    .WithReference(apiService);

builder.Build().Run();
