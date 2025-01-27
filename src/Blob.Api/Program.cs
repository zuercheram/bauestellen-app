using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Baustellen.App.Shared.Constants;
using Microsoft.AspNetCore.Http.HttpResults;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.AddAzureBlobClient(AppConstants.AzureStorageBlob);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.MapPost("/upload/{containerName}", UploadBlob)
.WithName("UploadImage");

app.MapGet("/stream-image/{containerName}/{blobName}", GetBlob);

app.Run();

static async Task<Results<Ok<BlobContentInfo>, BadRequest<string>>> UploadBlob(IFormFile file, string containerName, BlobServiceClient blobServiceClient, CancellationToken token)
{
    if (file == null || file.Length == 0)
    {
        return TypedResults.BadRequest("No file in request found!");
    }
    using var stream = new MemoryStream();
    await file.CopyToAsync(stream, token);
    var containerClient = blobServiceClient.GetBlobContainerClient(containerName);
    await containerClient.CreateIfNotExistsAsync(cancellationToken: token);
    var blobClient = containerClient.GetBlobClient(file.FileName);
    try
    {
        var blobInfo = await blobClient.UploadAsync(stream, token);
        return TypedResults.Ok(blobInfo.Value);
    }
    catch (Exception ex)
    {
        return TypedResults.BadRequest($"File with name {file.FileName} already exists!");
    }
}

static async Task<Results<FileStreamHttpResult, NotFound>> GetBlob(string blobName, string containerName, BlobServiceClient blobServiceClient, CancellationToken token)
{
    var blobContainerClient = blobServiceClient.GetBlobContainerClient(containerName);
    var blobClient = blobContainerClient.GetBlobClient(blobName);
    if (await blobClient.ExistsAsync(token))
    {
        return TypedResults.Stream(await blobClient.OpenReadAsync(cancellationToken: token), "image/jpeg");
    }
    return TypedResults.NotFound();   
}