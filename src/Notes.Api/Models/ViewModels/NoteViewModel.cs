namespace Baustellen.App.Notes.Api.Models.ViewModels;

public class NoteViewModel
{
    public Guid Id { get; set; }
    public string AuthorName { get; set; } = string.Empty;    
    public string CreatedAt { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
    public IList<ImageViewModel> Images { get; set; } = [];
}
