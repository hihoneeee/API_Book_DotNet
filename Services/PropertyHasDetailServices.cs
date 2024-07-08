using AutoMapper;
using TestWebAPI.DTOs.Property;
using TestWebAPI.Helpers;
using TestWebAPI.Models;
using TestWebAPI.Repositories.Interfaces;
using TestWebAPI.Response;
using TestWebAPI.Services.Interfaces;

namespace TestWebAPI.Services
{
    public class PropertyHasDetailServices : IPropertyHasDetailServices
    {
        private readonly IPropertyHasDetailRepositories _propertyHasDetailRepo;
        private readonly ICloudinaryServices _cloudinaryServices;
        private readonly IMapper _mapper;

        public PropertyHasDetailServices(IMapper mapper, IPropertyHasDetailRepositories propertyHasDetailRepo, ICloudinaryServices cloudinaryServices)
        {
            _propertyHasDetailRepo = propertyHasDetailRepo;
            _cloudinaryServices = cloudinaryServices;
            _mapper = mapper;
        }

        public async Task<ServiceResponse<PropertyHasDetailDTO>> CreateDetailAsync(int propertyId, PropertyHasDetailDTO propertyHasDetailDTO)
        {
            var serviceResponse = new ServiceResponse<PropertyHasDetailDTO>();
            var imagePublicIds = new List<string>();
            try
            {
                // Upload property has detail images
                var imagePaths = new List<string>();
                foreach (var image in propertyHasDetailDTO.images)
                {
                    var uploadResult = await _cloudinaryServices.UploadImage(image);
                    if (uploadResult == null || string.IsNullOrEmpty(uploadResult.Url.ToString()))
                    {
                        throw new Exception("Image upload failed");
                    }
                    imagePaths.Add(uploadResult.Url.ToString());
                    imagePublicIds.Add(uploadResult.PublicId);
                }

                var propertyHasDetail = _mapper.Map<PropertyHasDetail>(propertyHasDetailDTO);
                propertyHasDetail.propertyId = propertyId;
                propertyHasDetail.images = imagePaths;
                // Create propertyHasDetail
                var createdPropertyHasDetail = await _propertyHasDetailRepo.CreateDetailyAsync(propertyHasDetail);

                serviceResponse.SetSuccess("Detail created successfully!");
            }
            catch (Exception ex)
            {
                serviceResponse.SetError(ex.Message);
                foreach (var publicId in imagePublicIds)
                {
                    await _cloudinaryServices.DeleteImage(publicId);
                }
            }
            return serviceResponse;
        }

        public async Task<ServiceResponse<PropertyHasDetailDTO>> UpdateDetailAsync(int id, PropertyHasDetailDTO propertyHasDetailDTO)
        {
            var serviceResponse = new ServiceResponse<PropertyHasDetailDTO>();
            var imagePublicIds = new List<string>();
            try
            {
                var checkExistDetail = await _propertyHasDetailRepo.GetDetailByIdAsync(id);
                if (checkExistDetail == null)
                {
                    serviceResponse.SetNotFound("Detail");
                    return serviceResponse;
                }

                var existingImagePublicIds = checkExistDetail.images.Select(img => _cloudinaryServices.ExtractPublicIdFromUrl(img)).ToList();
                foreach (var publicId in existingImagePublicIds)
                {
                    await _cloudinaryServices.DeleteImage(publicId);
                }

                var imagePaths = new List<string>();
                foreach (var image in propertyHasDetailDTO.images)
                {
                    var uploadResult = await _cloudinaryServices.UploadImage(image);
                    if (uploadResult == null || string.IsNullOrEmpty(uploadResult.Url.ToString()))
                    {
                        throw new Exception("Image upload failed");
                    } 
                    imagePaths.Add(uploadResult.Url.ToString());
                    imagePublicIds.Add(uploadResult.PublicId);
                }

                var propertyHasDetail = _mapper.Map<PropertyHasDetail>(propertyHasDetailDTO);
                propertyHasDetail.images = imagePaths;
                var createdPropertyHasDetail = await _propertyHasDetailRepo.UpdateDetailyAsync(checkExistDetail, propertyHasDetail);
                serviceResponse.SetSuccess("Detail updated successfully!");
            }
            catch (Exception ex)
            {
                serviceResponse.SetError(ex.Message);
                foreach (var publicId in imagePublicIds)
                {
                    await _cloudinaryServices.DeleteImage(publicId);
                }
            }
            return serviceResponse;
        }

        public async Task<ServiceResponse<PropertyHasDetail>> DeletePropertyAsync(int id)
        {
            var serviceResponse = new ServiceResponse<PropertyHasDetail>();
            try
            {
                var checkExist = await _propertyHasDetailRepo.GetDetailByIdAsync(id);
                if (checkExist == null)
                {
                    serviceResponse.SetNotFound("Detail");
                    return serviceResponse;
                }

                var imagePublicIds = checkExist.images.Select(img => _cloudinaryServices.ExtractPublicIdFromUrl(img)).ToList();
                foreach (var publicId in imagePublicIds)
                {
                    await _cloudinaryServices.DeleteImage(publicId);
                }
                await _propertyHasDetailRepo.DeleteDetailAsync(checkExist);

                serviceResponse.SetSuccess("Detail deleted successfully!");
            }
            catch (Exception ex)
            {
                serviceResponse.SetError(ex.Message);
            }
            return serviceResponse;
        }
    }
}
