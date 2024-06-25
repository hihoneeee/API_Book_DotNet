using Microsoft.AspNetCore.Mvc;
using TestWebAPI.DTOs.Category;
using TestWebAPI.DTOs.Property;
using TestWebAPI.Services;
using TestWebAPI.Services.Interfaces;
using TestWebAPI.Settings;
using static TestWebAPI.Response.HttpStatus;

namespace TestWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class propertyController : ControllerBase
    {
        private readonly ICloudinaryServices _cloudinaryServices;
        private readonly IPropertyServices _propertyServices;

        public propertyController(ICloudinaryServices cloudinaryServices, IPropertyServices propertyServices)
        {
            _cloudinaryServices = cloudinaryServices;
            _propertyServices = propertyServices;
        }

        //[Authorize(Policy = "create-poperty")]
        [HttpPost]
        public async Task<IActionResult> CreateCategoryAsync([FromForm] PropertyDTO propertyDTO, [FromForm] PropertyHasDetailDTO propertyHasDetailDTO)
        {
            var uploadResult = await _cloudinaryServices.UploadImage(propertyDTO.avatar);
            if (uploadResult == null || string.IsNullOrEmpty(uploadResult.Url.ToString()))
            {
                return StatusCode(500, "Image upload failed");
            }
            var path = uploadResult.Url.ToString();
            var publicId = uploadResult.PublicId;

            var serviceResponse = await _propertyServices.CreatePropertyAsync(propertyDTO, propertyHasDetailDTO, path, publicId);
            if (serviceResponse.statusCode == EHttpType.Success)
            {
                return Ok(new { serviceResponse.success, serviceResponse.message });
            }
            else
            {
                return StatusCode((int)serviceResponse.statusCode, new { serviceResponse.success, serviceResponse.message });
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetPropertiesAsync([FromQuery] QueryParamsSetting queryParams)
        {
            var serviceResponse = await _propertyServices.GetPropertiesAsync(queryParams);
            if (serviceResponse.statusCode == EHttpType.Success)
            {
                return Ok(new
                {
                    serviceResponse.success,
                    serviceResponse.message,
                    serviceResponse.total,
                    serviceResponse.limit,
                    serviceResponse.page,
                    serviceResponse.data

                });
            }
            else
            {
                return StatusCode((int)serviceResponse.statusCode, new { serviceResponse.success, serviceResponse.message });
            }
        }
    }
}
