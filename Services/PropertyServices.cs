using AutoMapper;
using TestWebAPI.DTOs.Property;
using TestWebAPI.Helpers;
using TestWebAPI.Models;
using TestWebAPI.Repositories.Interfaces;
using TestWebAPI.Response;
using TestWebAPI.Services.Interfaces;
using TestWebAPI.Settings;
using System.Linq.Dynamic.Core;
using Microsoft.EntityFrameworkCore;
using TestWebAPI.Configs;
using Newtonsoft.Json;
using CloudinaryDotNet.Actions;
using TestWebAPI.DTOs.Category;

namespace TestWebAPI.Services
{
    public class PropertyServices : IPropertyServices
    {
        private readonly IMapper _mapper;
        private readonly IPropertyRepositories _propertyRepo;
        private readonly ICloudinaryServices _cloudinaryServices;
        private readonly IPropertyHasDetailServices _propertyHasDetailServices;
        private readonly RedisCacheConfig _redisCacheConfig;

        public PropertyServices(IMapper mapper, IPropertyRepositories propertyRepo, ICloudinaryServices cloudinaryServices, IPropertyHasDetailServices propertyHasDetailServices, RedisCacheConfig redisCacheConfig)
        {
            _mapper = mapper;
            _propertyRepo = propertyRepo;
            _cloudinaryServices = cloudinaryServices;
            _propertyHasDetailServices = propertyHasDetailServices;
            _redisCacheConfig = redisCacheConfig;
        }

        public async Task<ServiceResponse<PropertyDTO>> CreatePropertyAsync(PropertyDTO propertyDTO, PropertyHasDetailDTO propertyHasDetailDTO)
        {
            var serviceResponse = new ServiceResponse<PropertyDTO>();
            var imagePublicIds = new List<string>();

            try
            {
                var property = _mapper.Map<Property>(propertyDTO);

                // Upload avatar to cloudinary
                var avatarUploadResult = await _cloudinaryServices.UploadImage(propertyDTO.avatar);
                if (avatarUploadResult == null || string.IsNullOrEmpty(avatarUploadResult.Url.ToString()))
                {
                  serviceResponse.SetError("Avatar upload failed");
                }
                property.avatar = avatarUploadResult.Url.ToString();
                imagePublicIds.Add(avatarUploadResult.PublicId);

                // Create property
                var createdProperty = await _propertyRepo.CreatePropertyAsync(property);
                await _propertyHasDetailServices.CreateDetailAsync(createdProperty.id, propertyHasDetailDTO);
                serviceResponse.SetSuccess("Property created successfully!");
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

        public async Task<ServiceResponse<PropertyDTO>> UpdatePropertyAsync(int id, PropertyDTO propertyDTO, PropertyHasDetailDTO propertyHasDetailDTO)
        {
            var serviceResponse = new ServiceResponse<PropertyDTO>();
            var imagePublicIds = new List<string>();
            try
            {
                // check id property
                var checkExist = await _propertyRepo.GetPropertyByIdAsync(id);
                if (checkExist == null)
                {
                    serviceResponse.SetNotFound("property");
                    return serviceResponse;
                }

                // find url avatar and extract url to publicID
                var oldImagePublicId = _cloudinaryServices.ExtractPublicIdFromUrl(checkExist.avatar);

                // delete publicID
                await _cloudinaryServices.DeleteImage(oldImagePublicId);

                // Upload avatar to cloudinary
                var property = _mapper.Map<Property>(propertyDTO);

                var avatarUploadResult = await _cloudinaryServices.UploadImage(propertyDTO.avatar);
                if (avatarUploadResult == null || string.IsNullOrEmpty(avatarUploadResult.Url.ToString()))
                {
                    serviceResponse.SetError("Avatar upload failed");
                }
                property.avatar = avatarUploadResult.Url.ToString();
                imagePublicIds.Add(avatarUploadResult.PublicId);

                // Update property
                var updatePorperty = await _propertyRepo.UpdatePropertyAsync(checkExist, property);
                await _propertyHasDetailServices.UpdateDetailAsync(checkExist.PropertyHasDetail.id, propertyHasDetailDTO);
                serviceResponse.SetSuccess("Property updated successfully!");
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

        public async Task<ServiceResponse<PropertyDTO>> DeletePropertyAsync(int id)
        {
            var serviceResponse = new ServiceResponse<PropertyDTO>();
            try
            {
                var checkExist = await _propertyRepo.GetPropertyByIdAsync(id);
                if (checkExist == null)
                {
                    serviceResponse.SetNotFound("Property");
                    return serviceResponse;
                }
                var publicId = _cloudinaryServices.ExtractPublicIdFromUrl(checkExist.avatar);
                await _cloudinaryServices.DeleteImage(publicId);
                if (checkExist.PropertyHasDetail != null)
                {
                    await _propertyHasDetailServices.DeletePropertyAsync(checkExist.PropertyHasDetail.id);
                }
                await _propertyRepo.DeletePropertyAsync(checkExist);
                serviceResponse.SetSuccess("Property deleted successfully!");
            }
            catch (Exception ex)
            {
                serviceResponse.SetError(ex.Message);
            }
            return serviceResponse;
        }

        public async Task<ServiceResponse<List<GetPropertyDTO>>> GetPropertiesAsync(QueryParamsSetting queryParams)
        {
            var serviceResponse = new ServiceResponse<List<GetPropertyDTO>>();
            try
            {
                // Generate cache key
                var cacheKey = CacheKeyGenerator.GenerateKey(queryParams, queryParams.sort, queryParams.fields, queryParams.page, queryParams.limit, queryParams.address);

                // Check Redis cache
                var cachedData = await _redisCacheConfig.GetCacheValueAsync(cacheKey);

                List<GetPropertyDTO> properties;
                int total;

                if (!string.IsNullOrEmpty(cachedData))
                {
                    // Deserialize cached data
                    var cachedResult = JsonConvert.DeserializeObject<CachedPropertiesDTO>(cachedData);
                    properties = cachedResult.Properties;
                    total = cachedResult.TotalCount;
                }
                else
                {
                    // Fetch from DB
                    var (query, totalFromDb) = await _propertyRepo.GetPropertiesAsync(queryParams);

                    total = totalFromDb;

                    if (!string.IsNullOrEmpty(queryParams.fields))
                    {
                        var fields = queryParams.fields.Split(',').Select(f => f.Trim()).ToList();
                        var isExclude = fields.Any(f => f.StartsWith("-"));

                        properties = await query
                            .Select(p => new GetPropertyDTO
                            {
                                id = isExclude ? (!fields.Contains("-id") ? p.id : 0) : (fields.Contains("id") ? p.id : 0),
                                title = isExclude ? (!fields.Contains("-title") ? p.title : null) : (fields.Contains("title") ? p.title : null),
                                description = isExclude ? (!fields.Contains("-description") ? p.description : null) : (fields.Contains("description") ? p.description : null),
                                price = isExclude ? (!fields.Contains("-price") ? p.price : 0) : (fields.Contains("price") ? p.price : 0),
                                avatar = isExclude ? (!fields.Contains("-avatar") ? p.avatar : null) : (fields.Contains("avatar") ? p.avatar : null),
                                categoryId = isExclude ? (!fields.Contains("-categoryId") ? p.categoryId : 0) : (fields.Contains("categoryId") ? p.categoryId : 0),
                                status = isExclude ? (!fields.Contains("-status") ? p.status : 0) : (fields.Contains("status") ? p.status : 0),
                                createdAt = isExclude ? (!fields.Contains("-createdAt") ? p.createdAt : DateTime.MinValue) : (fields.Contains("createdAt") ? p.createdAt : DateTime.MinValue),
                                updatedAt = isExclude ? (!fields.Contains("-updatedAt") ? p.updatedAt : DateTime.MinValue) : (fields.Contains("updatedAt") ? p.updatedAt : DateTime.MinValue),
                                dataDetail = isExclude ?
                                    (!fields.Contains("-dataDetail") && p.PropertyHasDetail != null ? new GetPropertyHasDetailDTO
                                    {
                                        id = p.PropertyHasDetail.id,
                                        sellerId = p.PropertyHasDetail.sellerId,
                                        propertyId = p.PropertyHasDetail.propertyId,
                                        province = p.PropertyHasDetail.province,
                                        city = p.PropertyHasDetail.city,
                                        images = p.PropertyHasDetail.images,
                                        address = p.PropertyHasDetail.address,
                                        bedroom = p.PropertyHasDetail.bedroom,
                                        bathroom = p.PropertyHasDetail.bathroom,
                                        yearBuild = p.PropertyHasDetail.yearBuild,
                                        size = p.PropertyHasDetail.size,
                                        type = p.PropertyHasDetail.type
                                    } : null) :
                                    (fields.Contains("dataDetail") && p.PropertyHasDetail != null ? new GetPropertyHasDetailDTO
                                    {
                                        id = p.PropertyHasDetail.id,
                                        sellerId = p.PropertyHasDetail.sellerId,
                                        propertyId = p.PropertyHasDetail.propertyId,
                                        province = p.PropertyHasDetail.province,
                                        city = p.PropertyHasDetail.city,
                                        images = p.PropertyHasDetail.images,
                                        address = p.PropertyHasDetail.address,
                                        bedroom = p.PropertyHasDetail.bedroom,
                                        bathroom = p.PropertyHasDetail.bathroom,
                                        yearBuild = p.PropertyHasDetail.yearBuild,
                                        size = p.PropertyHasDetail.size,
                                        type = p.PropertyHasDetail.type
                                    } : null)
                            })
                            .ToListAsync();
                    }
                    else
                    {
                        properties = await query
                            .Select(p => new GetPropertyDTO
                            {
                                id = p.id,
                                title = p.title,
                                description = p.description,
                                price = p.price,
                                avatar = p.avatar,
                                categoryId = p.categoryId,
                                status = p.status,
                                createdAt = p.createdAt,
                                updatedAt = p.updatedAt,
                                dataDetail = p.PropertyHasDetail != null ? new GetPropertyHasDetailDTO
                                {
                                    id = p.PropertyHasDetail.id,
                                    sellerId = p.PropertyHasDetail.sellerId,
                                    propertyId = p.PropertyHasDetail.propertyId,
                                    province = p.PropertyHasDetail.province,
                                    city = p.PropertyHasDetail.city,
                                    images = p.PropertyHasDetail.images,
                                    address = p.PropertyHasDetail.address,
                                    bedroom = p.PropertyHasDetail.bedroom,
                                    bathroom = p.PropertyHasDetail.bathroom,
                                    yearBuild = p.PropertyHasDetail.yearBuild,
                                    size = p.PropertyHasDetail.size,
                                    type = p.PropertyHasDetail.type
                                } : null
                            })
                            .ToListAsync();
                    }

                    // Save to Redis cache
                    var resultToCache = new CachedPropertiesDTO
                    {
                        Properties = properties,
                        TotalCount = total
                    };
                    var serializedData = JsonConvert.SerializeObject(resultToCache);
                    await _redisCacheConfig.SetCacheValueAsync(cacheKey, serializedData, TimeSpan.FromMinutes(60));
                }

                serviceResponse.data = properties;
                serviceResponse.limit = queryParams.limit ?? total;
                serviceResponse.total = total;
                serviceResponse.page = queryParams.page ?? 1;
                serviceResponse.SetSuccess("Properties retrieved successfully!");
            }
            catch (Exception ex)
            {
                serviceResponse.SetError(ex.Message);
            }
            return serviceResponse;
        }
    }
}