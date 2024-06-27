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

namespace TestWebAPI.Services
{
    public class PropertyServices : IPropertyServices
    {
        private readonly IMapper _mapper;
        private readonly IPropertyRepositories _propertyRepo;
        private readonly ICloudinaryServices _cloudinaryServices;
        private readonly IPropertyHasDetailRepositories _propertyHasDetailRepo;
        private readonly RedisCacheConfig _redisCacheConfig;

        public PropertyServices(IMapper mapper, IPropertyRepositories propertyRepo, ICloudinaryServices cloudinaryServices, IPropertyHasDetailRepositories propertyHasDetailRepo, RedisCacheConfig redisCacheConfig)
        {
            _mapper = mapper;
            _propertyRepo = propertyRepo;
            _cloudinaryServices = cloudinaryServices;
            _propertyHasDetailRepo = propertyHasDetailRepo;
            _redisCacheConfig = redisCacheConfig;
        }

        public async Task<ServiceResponse<PropertyDTO>> CreatePropertyAsync(PropertyDTO propertyDTO, PropertyHasDetailDTO propertyHasDetailDTO, string path, string publicId)
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