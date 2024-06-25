using AutoMapper;
using System.IO;
using TestWebAPI.DTOs.Category;
using TestWebAPI.DTOs.Property;
using TestWebAPI.Helpers;
using TestWebAPI.Models;
using TestWebAPI.Repositories.Interfaces;
using TestWebAPI.Response;
using TestWebAPI.Services.Interfaces;
using TestWebAPI.Settings;

namespace TestWebAPI.Services
{
    public class PropertyServices : IPropertyServices
    {
        private readonly IMapper _mapper;
        private readonly IPropertyRepositories _propertyRepo;
        private readonly ICloudinaryServices _cloudinaryServices;
        private readonly IPropertyHasDetailRepositories _propertyHasDetailRepo;

        public PropertyServices(IMapper mapper, IPropertyRepositories propertyRepo, ICloudinaryServices cloudinaryServices, IPropertyHasDetailRepositories propertyHasDetailRepo) {
            _mapper = mapper;
            _propertyRepo = propertyRepo;
            _cloudinaryServices = cloudinaryServices;
            _propertyHasDetailRepo = propertyHasDetailRepo;
        }

        public async Task<ServiceResponse<PropertyDTO>> CreatePropertyAsync(PropertyDTO propertyDTO, PropertyHasDetailDTO propertyHasDetailDTO,string path, string publicId)
        {
            var serviceResponse = new ServiceResponse<PropertyDTO>();
            try
            {
                var property = _mapper.Map<Property>(propertyDTO);
                property.avatar = path;
                var createdProperty = await _propertyRepo.CreatePropertyAsync(property);
                var propertyHasDetail = _mapper.Map<PropertyHasDetail>(propertyHasDetailDTO);
                propertyHasDetail.propertyId = createdProperty.id;
                var createdPropertyHasDetail = await _propertyHasDetailRepo.CreatePropertyAsync(propertyHasDetail);

                serviceResponse.SetSuccess("Property created successfully!");
            }
            catch (Exception ex)
            {
                serviceResponse.SetError(ex.Message);
                await _cloudinaryServices.DeleteImage(publicId);
            }
            return serviceResponse;
        }

        public async Task<ServiceResponse<PropertyDTO>> UpdatePropertyAsync(int id, PropertyDTO propertyDTO)
        {
            throw new NotImplementedException();
        }

        public async Task<ServiceResponse<PropertyDTO>> DeletePropertyAsync(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<ServiceResponse<List<GetPropertyDTO>>> GetPropertiesAsync(QueryParamsSetting queryParams)
        {
            var serviceResponse = new ServiceResponse<List<GetPropertyDTO>>();
            try
            {
                var (properties, total) = await _propertyRepo.GetPropertiesAsync(queryParams);
                serviceResponse.data = _mapper.Map<List<GetPropertyDTO>>(properties);
                serviceResponse.limit = queryParams.limit ?? total;
                serviceResponse.total = total;
                serviceResponse.page = queryParams.page ?? 1;
                serviceResponse.SetSuccess("Properties got successfully!");
            }
            catch (Exception ex)
            {
                serviceResponse.SetError(ex.Message);
            }
            return serviceResponse;
        }
    }
}
