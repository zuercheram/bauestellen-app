namespace Baustellen.App.Notes.Api.Models.InputModels;
public class NoteInputModel
{    
    public string AuthorName { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
    public Guid ProjectId { get; set; }
    public IList<ImageInputModel> Images { get; set; } = [];
}
