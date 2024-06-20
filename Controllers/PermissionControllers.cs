using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TestWebAPI.DTOs.Permisstion;
using TestWebAPI.Services.Interfaces;
using static TestWebAPI.Response.HttpStatus;

namespace TestWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class permissionController : ControllerBase
    {
        private readonly IPermissionServices _permissionServices;
        public permissionController(IPermissionServices permissionServices)
        {
            _permissionServices = permissionServices;
        }
        [Authorize(Policy = "get-permission")]
        [HttpGet]
        public async Task<IActionResult> GetAllPermissonAsync()
        {
            var serviceResponse = await _permissionServices.GetAllPermissionAsyn();
            if (serviceResponse.statusCode == EHttpType.Success)
            {
                return Ok(new { serviceResponse.success, serviceResponse.message, serviceResponse.data });
            }
            else
            {
                return StatusCode((int)serviceResponse.statusCode, new { serviceResponse.success, serviceResponse.message });
            }
        }

        [Authorize(Policy = "add-permission")]
        [HttpPost]
        public async Task<IActionResult> CreatePermissionAsync([FromBody] AddPermissionDTO permisstionDTO)
        {
            var serviceResponse = await _permissionServices.CreatePermissionAsyn(permisstionDTO);
            if (serviceResponse.statusCode == EHttpType.Success)
            {
                return Ok(new { serviceResponse.success, serviceResponse.message });
            }
            else
            {
                return StatusCode((int)serviceResponse.statusCode, new { serviceResponse.success, serviceResponse.message });
            }
        }

        [Authorize(Policy = "update-permission")]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdatePermissionAsync(int id, PermisstionDTO permisstionDTO)
        {
            var serviceResponse = await _permissionServices.UpdatePermissionAsyn(id, permisstionDTO);
            if (serviceResponse.statusCode == EHttpType.Success)
            {
                return Ok(new { serviceResponse.success, serviceResponse.message });
            }
            else
            {
                return StatusCode((int)serviceResponse.statusCode, new { serviceResponse.success, serviceResponse.message });
            }

        }

        [Authorize(Policy = "delete-permission")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePermissionAsync(int id)
        {
            var serviceResponse = await _permissionServices.DeletePermissionAsyn(id);
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
