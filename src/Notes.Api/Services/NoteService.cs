using Baustellen.App.Notes.Api.Models;
using Baustellen.App.Notes.Api.Models.InputModels;
using Baustellen.App.Notes.Api.Models.ViewModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Notes.Api.Data;

namespace Baustellen.App.Notes.Api.Services;

public class NoteService(NotesDbContext dbContext, ImageService imageService)
{
    public async Task<IList<NoteViewModel>> GetNotes(Guid projectId)
    {
        return await dbContext.Notes.Where(x => x.ProjectId == projectId).Select(x => new NoteViewModel
        {
            Id = x.Id,
            AuthorName = x.AuthorName,
            Content = x.Content ?? string.Empty,
            CreatedAt = x.CreatedAt.ToString("dd.MM.yyyy HH.mm.ss"),
            Images = x.Images == null ? new List<ImageViewModel>() : x.Images.Select(i => new ImageViewModel
            {
                BlobContainer = i.BlobContainer ?? string.Empty,
                FileName = i.FileName ?? string.Empty,
                ImageUri = i.ImageUri ?? string.Empty
            }).ToList()
        }).ToListAsync();
    }

    public async Task SaveNote(NoteInputModel model, Guid noteId)
    {
        var note = await dbContext.Notes.FindAsync(noteId);
        if (note == null)
        {
            note = new Note
            {
                AuthorName = model.AuthorName,
                ProjectId = model.ProjectId
            };
            dbContext.Notes.Add(note);
        }

        note.Content = model.Content;
        note.Images
    }
}
