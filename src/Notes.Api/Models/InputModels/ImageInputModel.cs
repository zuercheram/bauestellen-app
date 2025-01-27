namespace Baustellen.App.Notes.Api.Models.InputModels;

public class ImageInputModel
{
    public string ImageUri { get; set; } = string.Empty;
    public string FileName { get; set; } = string.Empty;
    public string BlobContainer { get; set; } = string.Empty;    
}
