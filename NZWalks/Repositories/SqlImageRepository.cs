using NZWalks.Controllers.Models.Domain;
using NZWalks.Data;

namespace NZWalks.Repositories
{
    public class SqlImageRepository : IImageRepository
    {
        private readonly IWebHostEnvironment webHostEnvironment;
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly NZWalksDBContext nZWalksDBContext;

        public SqlImageRepository(IWebHostEnvironment webHostEnvironment,
            IHttpContextAccessor httpContextAccessor, 
            NZWalksDBContext nZWalksDBContext
            )
        {
            this.webHostEnvironment = webHostEnvironment;
            this.httpContextAccessor = httpContextAccessor;
            this.nZWalksDBContext = nZWalksDBContext;
        }
        public async Task<Image> UploadImageAsync(Image image)
        {
            var localFilePath = Path.Combine(webHostEnvironment.ContentRootPath, "Images",
                image.FileName,image.FileDescription);

            var fileStream = new FileStream(localFilePath, FileMode.Create);
            await image.File.CopyToAsync(fileStream);

            var imageUrl = $"{httpContextAccessor.HttpContext.Request.Scheme}://" +
                $"{httpContextAccessor.HttpContext.Request.Host}{httpContextAccessor.HttpContext.Request.PathBase}/Images/" +
                $"{image.FileName}{image.FileExtension}";

            image.FilePath = imageUrl;

            await nZWalksDBContext.Images.AddAsync(image);
            await nZWalksDBContext.SaveChangesAsync();

           

            return image;
        }
    }
}
