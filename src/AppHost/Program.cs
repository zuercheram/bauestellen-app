using Baustellen.App.AppHost.Extensions;
using Baustellen.App.Shared.Constants;
using Projects;

var builder = DistributedApplication.CreateBuilder(args);

builder.AddForwardedHeaders();

var postgres = builder.AddPostgres(AppConstants.PostgresServerName)
    .WithPgAdmin()
    .WithDataVolume(AppConstants.PostgresDataVolumeName, isReadOnly: false);

var projectDb = postgres.AddDatabase(AppConstants.PostgresProjectDatabaseName);
var noteDb = postgres.AddDatabase(AppConstants.PostgresNoteDatabaseName);
var identityDb = postgres.AddDatabase(AppConstants.PostgresIdentityDatabaseName);

var blob = builder.AddAzureStorage(AppConstants.AzureStorageResource)
    .RunAsEmulator()
    .AddBlobs(AppConstants.AzureStorageBlob);

var launchProfileName = "https";

var identityApi = builder.AddProject<Baustellen_App_Identity_Api>(AppConstants.IdentityApiProject, launchProfileName)
    .WaitFor(identityDb)
    .WithReference(identityDb)
    .WithExternalHttpEndpoints();

var projectApi = builder.AddProject<Baustellen_App_Projects_Api>(AppConstants.AppApiProject, launchProfileName)
    .WaitFor(projectDb)    
    .WithReference(projectDb)
    .WithReference(identityApi)
    .WithExternalHttpEndpoints();

var blobApi = builder.AddProject<Baustellen_App_Blob_Api>(AppConstants.AppApiBlob, launchProfileName)
    .WaitFor(blob)
    .WithReference(blob);

var notesApi = builder.AddProject<Baustellen_App_Notes_Api>(AppConstants.AppApiNotes, launchProfileName)
    .WaitFor(noteDb)    
    .WithReference(noteDb)
    .WithReference(identityApi)
    .WithReference(projectApi)
    .WithReference(blobApi)
    .WithExternalHttpEndpoints();

projectApi.WithReference(notesApi);

var mobileApp = builder.AddMobileProject(AppConstants.MobileAppProject, "../Client", clientStubProjectPath: "../ClientStub/Baustellen.App.ClientStub.csproj")
    .WithReference(projectApi);

var webclient = builder.AddNpmApp(AppConstants.WebClientProject, "../WebClient")
    .WithReference(projectApi)    
    .WaitFor(projectApi)
    .WithHttpsEndpoint(env: "PORT")
    .WithExternalHttpEndpoints()    
    .PublishAsDockerFile();

var gateway = builder.AddProject<Baustellen_App_Gateway>(AppConstants.GatewayProject)    
    .WithReference(projectApi)
    .WithReference(notesApi)
    .WithReference(identityApi)
    .WithReference(webclient)
    .WithReference(blobApi);

mobileApp.WithReference(gateway);
webclient.WithReference(gateway);

builder.Build().Run();
