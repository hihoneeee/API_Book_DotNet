using Microsoft.AspNetCore.Mvc;
using TestWebAPI.Data;
using TestWebAPI.Helpers;
using TestWebAPI.Repositories.Interfaces;
using TestWebAPI.DTOs.Role;
using AutoMapper;
using TestWebAPI.Services.Interfaces;
using static TestWebAPI.Response.HttpStatus;
using Microsoft.AspNetCore.Authorization;
using TestWebAPI.Services;
using TestWebAPI.DTOs.RoleHasPermission;

namespace TestWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RolesController : ControllerBase
    {
        private readonly IRoleService _roleService;
        private readonly IRoleHasPermissionServices _roleHasPermissionSv;

        public RolesController( IRoleService roleService, IRoleHasPermissionServices roleHasPermissionSv)
        {
            _roleService = roleService;
            _roleHasPermissionSv = roleHasPermissionSv;
        }

        // GET: api/Roles
        [Authorize(Policy = "get-role")]
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
        [Authorize(Policy = "get-only-role")]
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
        [Authorize(Policy = "update-role")]
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
        [Authorize(Policy = "add-role")]
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
        [Authorize(Policy = "delete-role")]
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
        [Route("assign-permission")]
        [HttpPost]
        public async Task<IActionResult> AssignPermissionAsyn([FromBody] RoleHasPermissionDTO roleHasPermissionDTO)
        {
            var serviceResponse = await _roleHasPermissionSv.AssignPermissionAsyn(roleHasPermissionDTO.codeRole, roleHasPermissionDTO.codePermission, roleHasPermissionDTO);
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
