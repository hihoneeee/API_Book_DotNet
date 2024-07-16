using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TestWebAPI.DTOs.Category;
using TestWebAPI.Services.Interfaces;
using static TestWebAPI.Response.HttpStatus;

namespace TestWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class categoryController : ControllerBase
    {
        private readonly ICategoryServices _categoryServices;

        public categoryController(ICategoryServices categoryServices)
        {
            _categoryServices = categoryServices;
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
        [HttpGet("{id}")]
        public async Task<IActionResult> GetCategoryByIdAsync(int id)
        {
            var serviceResponse = await _categoryServices.GetCategoryByIdAsync(id);
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
            var serviceResponse = await _categoryServices.CreateCategoryAsync(categoryDTO);
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
            var serviceResponse = await _categoryServices.UpdateCategoryAsync(id, categoryDTO);
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
