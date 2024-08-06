using Microsoft.AspNetCore.Mvc;
using TestWebAPI.DTOs.Role;
using TestWebAPI.Services.Interfaces;
using static TestWebAPI.Response.HttpStatus;
using Microsoft.AspNetCore.Authorization;
using TestWebAPI.DTOs.RoleHasPermission;

namespace TestWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    // [Authorize(Roles = "D22MD2")]
    public class roleController : ControllerBase
    {
        private readonly IRoleService _roleService;
        private readonly IRoleHasPermissionServices _roleHasPermissionSv;

        public roleController( IRoleService roleService, IRoleHasPermissionServices roleHasPermissionSv)
        {
            _roleService = roleService;
            _roleHasPermissionSv = roleHasPermissionSv;
        }

        // GET: api/Roles
        //[Authorize(Policy = "get-role")]
        [HttpGet]
        public async Task<IActionResult> GetAllRoles()
        {
            var serviceResponse = await _roleService.GetAllRoles();
            if (serviceResponse.statusCode == EHttpType.Success)
            {
                return Ok(new { serviceResponse.success, serviceResponse.message, serviceResponse.data });
            }
            else
            {
                return StatusCode((int)serviceResponse.statusCode, new { serviceResponse.success, serviceResponse.message });
            }
        }

        // GET: api/Roles/5
        // [Authorize(Policy = "get-only-role")]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetRolesById(int id)
        {
            var serviceResponse = await _roleService.GetRolesById(id);
            if (serviceResponse.statusCode == EHttpType.Success)
            {
                return Ok(new { serviceResponse.success, serviceResponse.message, serviceResponse.data });
            }
            else
            {
                return StatusCode((int)serviceResponse.statusCode, new { serviceResponse.success, serviceResponse.message });
            }
        }

        // PUT: api/Roles/5
        // [Authorize(Policy = "update-role")]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateRoleAsync(int id, AddRoleDTO roleDTO)
        {
            var serviceResponse = await _roleService.UpdateRoleAsync(id, roleDTO);
            if (serviceResponse.statusCode == EHttpType.Success)
            {
                return Ok(new { serviceResponse.success, serviceResponse.message });
            }
            else
            {
                return StatusCode((int)serviceResponse.statusCode, new { serviceResponse.success, serviceResponse.message });
            }

        }

        // POST: api/Roles
        //[Authorize(Policy = "add-role")]
        [HttpPost]
        public async Task<IActionResult> AddNewRole([FromBody] AddRoleDTO roleDTO)
        {
            var serviceResponse = await _roleService.AddRoleAsync(roleDTO);
            if (serviceResponse.statusCode == EHttpType.Success)
            {
                return Ok(new { serviceResponse.success, serviceResponse.message });
            }
            else
            {
                return StatusCode((int)serviceResponse.statusCode, new { serviceResponse.success, serviceResponse.message });
            }
        }


        // DELETE: api/Roles/5
        // [Authorize(Policy = "delete-role")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRole(int id)
        {
                var serviceResponse = await _roleService.DeleteRoleAsync(id);
                if (serviceResponse.statusCode == EHttpType.Success)
                {
                    return Ok(new { serviceResponse.success, serviceResponse.message });
                }
                else
                {
                    return StatusCode((int)serviceResponse.statusCode, new { serviceResponse.success, serviceResponse.message });
                }
        }
        //[Authorize(Policy = "assign-permission")]
        [Route("assign-permission")]
        [HttpPost]
        public async Task<IActionResult> AssignPermissionAsyn([FromBody] AddRoleHasPermissionDTO addRoleHasPermissionDTO)
        {
            var serviceResponse = await _roleHasPermissionSv.AssignPermissionsAsync(addRoleHasPermissionDTO);
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
