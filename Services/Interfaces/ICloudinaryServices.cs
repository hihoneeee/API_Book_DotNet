using CloudinaryDotNet.Actions;
using TestWebAPI.DTOs.Common;

namespace TestWebAPI.Services.Interfaces
{
    public interface ICloudinaryServices
    {
        Task<ImageUploadResult> UploadImage(IFormFile file);
        Task<DeletionResult> DeleteImage(string publicId);
        string ExtractPublicIdFromUrl(string path);
    }
}
