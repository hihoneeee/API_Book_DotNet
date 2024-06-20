using CloudinaryDotNet.Actions;

namespace TestWebAPI.Services.Interfaces
{
    public interface ICloudinaryServices
    {
        Task<ImageUploadResult> UploadImage(IFormFile file);
        Task<DeletionResult> DeleteImage(string publicId);
    }
}
