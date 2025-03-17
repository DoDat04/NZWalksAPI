using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NZWalks.API.Models.Domain;
using NZWalks.API.Models.DTO;
using NZWalks.API.Repositories;

namespace NZWalks.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ImagesController : ControllerBase
    {
        private readonly IMapper mapper;
        private readonly IImageRepository localImageRepository;
        private readonly AzureImageRepository azureImageRepository; // Không dùng IImageRepository

        public ImagesController(IMapper mapper, IImageRepository localImageRepository, AzureImageRepository azureImageRepository)
        {
            this.mapper = mapper;
            this.localImageRepository = localImageRepository;
            this.azureImageRepository = azureImageRepository;
        }
        // POST: /api/Images/Upload
        [HttpPost]
        [Route("Upload")]
        public async Task<IActionResult> Upload([FromForm] ImageUploadRequestDto imageUploadRequestDto, [FromQuery] bool useAzure = false)
        {
            ValidateFileUpload(imageUploadRequestDto);

            if (ModelState.IsValid)
            {
                var imageDomainModel = new Image
                {
                    File = imageUploadRequestDto.File,
                    FileExtension = Path.GetExtension(imageUploadRequestDto.File.FileName),
                    FileSizeInBytes = imageUploadRequestDto.File.Length,
                    FileName = $"{Guid.NewGuid()}{Path.GetExtension(imageUploadRequestDto.File.FileName)}",
                    FileDescription = imageUploadRequestDto.FileDescription
                };

                // Chọn repository phù hợp
                var repository = useAzure ? azureImageRepository : localImageRepository;
                var uploadedImage = await repository.Upload(imageDomainModel);

                return Ok(new { uploadedImage.FilePath });
            }

            return BadRequest(ModelState);
        }

        private void ValidateFileUpload(ImageUploadRequestDto imageUploadRequestDto)
        {
            var allowedExtension = new string[] { ".jpg", ".jpeg", ".png" };

            if (!allowedExtension.Contains(Path.GetExtension(imageUploadRequestDto.File.FileName)))
            {
                ModelState.AddModelError("file", "Unsupported file extension");
            }

            if (imageUploadRequestDto.File.Length > 10485760)
            {
                ModelState.AddModelError("file", "File size more than 10MB, please upload a smaller size file");
            }
        }
    }
}
