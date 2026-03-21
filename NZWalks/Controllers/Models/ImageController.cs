using System.Runtime.ConstrainedExecution;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NZWalks.Controllers.Models.Domain;
using NZWalks.Controllers.Models.DTO;
using NZWalks.Repositories;

namespace NZWalks.Controllers.Models
{
    [Route("api/[controller]")]
    [ApiController]
    public class ImageController : ControllerBase
    {

        private readonly IImageRepository imageRepository;
        public ImageController(IImageRepository imageRepository)
        {
            this.imageRepository = imageRepository;
        }

        [HttpPost]
        [Route("Upload")]
        public async Task<IActionResult> UploadImage([FromForm] ImageDTO incomingImageDTO)
        {
            if (ValidateImage(incomingImageDTO))
            {
                var image = new Image
                {
                    File = incomingImageDTO.File,
                    FileName = incomingImageDTO.FileName,
                    FileDescription = incomingImageDTO.FileDescription,
                    FileSizeInBytes = incomingImageDTO.File.Length,
                    FileExtension = Path.GetExtension(incomingImageDTO.File.FileName)
                };

                var insertedImage = await imageRepository.UploadImageAsync(image);

                return Ok(insertedImage);
            }
            else
            {
                return BadRequest(ModelState);
            }
        }

        public bool ValidateImage(ImageDTO incomingImageDTO)
        {
            string[] validFormats = { ".jpg", "jpeg", ".png", ".gif", ".bmp" };
            if (!validFormats.Contains(Path.GetExtension(incomingImageDTO.File.FileName)))
            {
                ModelState.AddModelError("File", "Invalid file format. Only .jpg, .jpeg, .png, .gif, and .bmp are allowed.");

            }

            if (incomingImageDTO.File.Length > 15728640)
            {
                ModelState.AddModelError("File", "File size exceeds the maximum allowed size of 15MB.");

            }
            return true;
        } 
    }
}
