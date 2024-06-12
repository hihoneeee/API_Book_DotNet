using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TestWebAPI.DTOs.Permisstion;
using TestWebAPI.DTOs.Role;
using TestWebAPI.Services;
using TestWebAPI.Services.Interfaces;
using static TestWebAPI.Response.HttpStatus;

namespace TestWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PermissionController : ControllerBase
    {
        private readonly IPermissionServices _permissionServices;
        public PermissionController(IPermissionServices permissionServices)
        {
            _permissionServices = permissionServices;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllRoles()
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

        [HttpPost]
        public async Task<IActionResult> CreatePermissionAsyn([FromBody] PermisstionDTO permisstionDTO)
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

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdatePermissionAsyn(int id, PermisstionDTO permisstionDTO)
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

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePermissionAsyn(int id)
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
