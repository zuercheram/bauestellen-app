using Baustellen.App.Notes.Api.Models;
using Baustellen.App.Notes.Api.Models.InputModels;

namespace Baustellen.App.Notes.Api.Services
{
    public class ImageService()
    {
        public async Task<Image> CreateImageEntity(ImageInputModel model, Note note)
        {
            return new Image
            {
                FileName = model.FileName,
                ImageUri = model.ImageUri,
                BlobContainer = model.BlobContainer,
            };
        }
    }
}
