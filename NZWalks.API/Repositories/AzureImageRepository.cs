using Azure.Storage.Blobs.Models;
using Azure.Storage.Blobs;
using NZWalks.API.Models.Domain;
using NZWalks.API.Data;

namespace NZWalks.API.Repositories
{
    public class AzureImageRepository : IImageRepository
    {
        private readonly BlobServiceClient _blobServiceClient;
        private readonly string _containerName;
        private readonly NZWalksDbContext dbContext; // Thêm database context

        public AzureImageRepository(IConfiguration configuration, NZWalksDbContext dbContext)
        {
            var connectionString = configuration["AzureStorage:ConnectionString"];
            _containerName = configuration["AzureStorage:ContainerName"];
            _blobServiceClient = new BlobServiceClient(connectionString);
            this.dbContext = dbContext;
        }
        //aa
        public async Task<Image> Upload(Image image)
        {
            var containerClient = _blobServiceClient.GetBlobContainerClient(_containerName);
            await containerClient.CreateIfNotExistsAsync(Azure.Storage.Blobs.Models.PublicAccessType.Blob);

            // Xóa phần extension để tránh bị lặp 
            var fileNameWithoutExtension = Path.GetFileNameWithoutExtension(image.FileName);
            var blobClient = containerClient.GetBlobClient($"{fileNameWithoutExtension}{image.FileExtension}");

            using (var stream = image.File.OpenReadStream())
            {
                await blobClient.UploadAsync(stream, new BlobHttpHeaders { ContentType = image.File.ContentType });
            }

            // Tạo URL file từ Azure
            var fileUrl = blobClient.Uri.ToString();

            // Lưu thông tin vào database
            var newImage = new Image
            {
                Id = Guid.NewGuid(),
                FileName = image.FileName,
                FileExtension = image.FileExtension,
                FileSizeInBytes = image.File.Length,
                FilePath = fileUrl, // Lưu URL thay vì đường dẫn cục bộ
                FileDescription = image.FileDescription
            };

            await dbContext.Images.AddAsync(newImage);
            await dbContext.SaveChangesAsync();

            newImage.File = null; // Không trả file khi response để tránh lỗi serialization
            return newImage;
        }
    }
}
