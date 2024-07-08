using Microsoft.AspNetCore.Mvc;
using TestWebAPI.DTOs.Property;
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
            var serviceResponse = await _propertyServices.CreatePropertyAsync(propertyDTO, propertyHasDetailDTO);
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

        [HttpPut]
        public async Task<IActionResult> UpdatePropertyAysnc(int id, [FromForm] PropertyDTO propertyDTO, [FromForm] PropertyHasDetailDTO propertyHasDetailDTO)
        {
            var serviceResponse = await _propertyServices.UpdatePropertyAsync(id, propertyDTO, propertyHasDetailDTO);
            if (serviceResponse.statusCode == EHttpType.Success)
            {
                return Ok(new { serviceResponse.success, serviceResponse.message });
            }
            else
            {
                return StatusCode((int)serviceResponse.statusCode, new { serviceResponse.success, serviceResponse.message });

            }
        }

        [HttpDelete]
        public async Task<IActionResult> DeletePropertyAsync(int id)
        {
            var serviceResponse = await _propertyServices.DeletePropertyAsync(id);
            if (serviceResponse.statusCode == EHttpType.Success)
            {
                return Ok(new { serviceResponse.success, serviceResponse.message });
            }
            else
            {
                return StatusCode((int)serviceResponse.statusCode, new { serviceResponse.success, serviceResponse.message });

            }
        }
    }
}