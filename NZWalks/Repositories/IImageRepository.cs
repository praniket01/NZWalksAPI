using NZWalks.Controllers.Models.Domain;

namespace NZWalks.Repositories
{
    public interface IImageRepository
    {
        Task<Image> UploadImageAsync(Image image);
    }
}
