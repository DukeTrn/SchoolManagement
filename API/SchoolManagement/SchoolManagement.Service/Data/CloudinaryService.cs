using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using SchoolManagement.Model;
using SchoolManagement.Service.Intention.Data;

namespace SchoolManagement.Service.Data
{
    public class CloudinaryService : ICloudinaryService
 {
        private readonly Cloudinary _cloudinary;

        public CloudinaryService(IConfiguration configuration)
        {
            var cloudinaryAccount = new CloudinaryAccount
            {
                CloudName = configuration["Cloudinary:CloudName"],
                ApiKey = configuration["Cloudinary:ApiKey"],
                ApiSecret = configuration["Cloudinary:ApiSecret"]
            };

            _cloudinary = new Cloudinary(new Account(cloudinaryAccount.CloudName, cloudinaryAccount.ApiKey, cloudinaryAccount.ApiSecret));
        }

        public async Task<string> UploadImageAsync(IFormFile file)
        {
            using (var stream = file.OpenReadStream())
            {
                var uploadParams = new ImageUploadParams
                {
                    File = new FileDescription(file.FileName, stream)
                };

                var uploadResult = await _cloudinary.UploadAsync(uploadParams);
                return uploadResult.SecureUrl.AbsoluteUri;
            }
        }

        public async Task<string> UploadImageAsync(string base64Image)
        {
            var imageBytes = Convert.FromBase64String(base64Image);
            using (var stream = new MemoryStream(imageBytes))
            {
                var uploadParams = new ImageUploadParams
                {
                    File = new FileDescription(Guid.NewGuid().ToString(), stream)
                };

                var uploadResult = await _cloudinary.UploadAsync(uploadParams);
                return uploadResult.SecureUrl.AbsoluteUri;
            }
        }
    }
}
