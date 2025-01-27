namespace Baustellen.App.Notes.Api.Models;
public class Image
{
    public Guid Id { get; set; }
    public string? ImageUri { get; set; }
    public string? FileName { get; set; }
    public string? BlobContainer { get; set; }
}
