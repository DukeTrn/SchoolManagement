using Microsoft.AspNetCore.Http;

namespace SchoolManagement.Service.Intention.Data
{
    public interface ICloudinaryService
    {
        Task<string> UploadImageAsync(string base64Image);
        Task<string> UploadImageAsync(IFormFile file);
    }
}
