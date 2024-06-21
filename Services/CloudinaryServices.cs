using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.Extensions.Options;
using TestWebAPI.DTOs.Common;
using TestWebAPI.Services.Interfaces;
using TestWebAPI.Settings;

namespace TestWebAPI.Services
{
    public class CloudinaryServices : ICloudinaryServices
    {
        private readonly Cloudinary _cloudinary;
        private readonly CloudinarySetting _cloudinarySetting;
        public CloudinaryServices(IOptions<CloudinarySetting> cloudinarySetting)
        {
            var settings = cloudinarySetting.Value;
            _cloudinary = new Cloudinary(new Account(settings.apiName, settings.apiKey, settings.apiSecret));
        }
        public async Task<ImageUploadResult> UploadImage(IFormFile file)
        {
            if (file.Length <= 0)
            {
                throw new ArgumentException("File is empty");
            }

            await using var stream = file.OpenReadStream();
            var uploadParams = new ImageUploadParams()
            {
                File = new FileDescription(file.FileName, stream)
            };

            var uploadResult = await _cloudinary.UploadAsync(uploadParams);
            return uploadResult;
        }
        public async Task<DeletionResult> DeleteImage(string publicId)
        {
            var deletionParams = new DeletionParams(publicId);
            var result = await _cloudinary.DestroyAsync(deletionParams);
            return result;
        }

        public string ExtractPublicIdFromUrl(string path)
        {
            var uri = new Uri(path);
            var segments = uri.AbsolutePath.Split('/');
            var publicIdWithExtension = segments[^1];
            var publicId = Path.GetFileNameWithoutExtension(publicIdWithExtension);
            return publicId;
        }
    }
}
