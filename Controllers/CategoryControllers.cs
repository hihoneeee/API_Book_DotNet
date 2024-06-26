using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TestWebAPI.DTOs.Category;
using TestWebAPI.DTOs.Common;
using TestWebAPI.Services;
using TestWebAPI.Services.Interfaces;
using static TestWebAPI.Response.HttpStatus;

namespace TestWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class categoryController : ControllerBase
    {
        private readonly ICategoryServices _categoryServices;
        private readonly ICloudinaryServices _cloudinaryServices;

        public categoryController(ICategoryServices categoryServices, ICloudinaryServices cloudinaryServices)
        {
            _categoryServices = categoryServices;
            _cloudinaryServices = cloudinaryServices;
        }
        // [Authorize(Policy = "get-category")]
        [HttpGet]
        public async Task<IActionResult> GetCategoriesAsync()
        {
            var serviceResponse = await _categoryServices.GetCategoriesAsync();
            if (serviceResponse.statusCode == EHttpType.Success)
            {
                return Ok(new { serviceResponse.success, serviceResponse.message, serviceResponse.data });
            }
            else
            {
                return StatusCode((int)serviceResponse.statusCode, new { serviceResponse.success, serviceResponse.message });
            }
        }
        //[Authorize(Policy = "create-category")]
        [HttpPost]
        public async Task<IActionResult> CreateCategoryAsync([FromForm] CategoryDTO categoryDTO)
        {
            var uploadResult = await _cloudinaryServices.UploadImage(categoryDTO.avatar);
            if (uploadResult == null || string.IsNullOrEmpty(uploadResult.Url.ToString()))
            {
                return StatusCode(500, "Image upload failed");
            }

            var path = uploadResult.Url.ToString();
            var publicId = uploadResult.PublicId;

            var serviceResponse = await _categoryServices.CreateCategoryAsync(categoryDTO, path, publicId);
            if (serviceResponse.statusCode == EHttpType.Success)
            {
                return Ok(new { serviceResponse.success, serviceResponse.message });
            }
            else
            {
                return StatusCode((int)serviceResponse.statusCode, new { serviceResponse.success, serviceResponse.message });
            }
        }

        //[Authorize(Policy = "update-category")]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCategoryAsync(int id, [FromForm] CategoryDTO categoryDTO)
        {
            var uploadResult = await _cloudinaryServices.UploadImage(categoryDTO.avatar);
            if (uploadResult == null || string.IsNullOrEmpty(uploadResult.Url.ToString()))
            {
                return StatusCode(500, "Image upload failed");
            }

            var path = uploadResult.Url.ToString();
            var publicId = uploadResult.PublicId;

            var serviceResponse = await _categoryServices.UpdateCategoryAsync(id, categoryDTO, path, publicId);
            if (serviceResponse.statusCode == EHttpType.Success)
            {
                return Ok(new { serviceResponse.success, serviceResponse.message });
            }
            else
            {
                return StatusCode((int)serviceResponse.statusCode, new { serviceResponse.success, serviceResponse.message });
            }
        }

        [Authorize(Policy = "delete-category")]
        [HttpDelete]
        public async Task<IActionResult> DeleteCategoryAsync(int id)
        {
            var serviceResponse = await _categoryServices.DeleteCategoryAsync(id);
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
