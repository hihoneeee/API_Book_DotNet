using Microsoft.AspNetCore.Mvc;
using TestWebAPI.Data;
using TestWebAPI.Helpers;
using TestWebAPI.Repositories.Interfaces;
using TestWebAPI.DTOs.Role;
using AutoMapper;
using TestWebAPI.Services.Interfaces;

namespace TestWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RolesController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IRoleService _roleService;
        public RolesController(ApplicationDbContext context, IRoleService roleService)
        {
            _context = context;
            _roleService = roleService;
        }

        // GET: api/Roles
        [HttpGet]
        public async Task<IActionResult> GetAllRoles()
        {
            try
            {
                var serviceResponse = await _roleService.GetAllRoles();
                if (serviceResponse.success)
                {
                    return Ok(serviceResponse.getData());
                }
                else
                {
                    return BadRequest(serviceResponse.getMessage());
                }
            }
            catch
            {
                throw new HttpStatusException(500, "Something went wrong");
            }
        }

        // GET: api/Roles/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetRolesById(int id)
        {
            try
            {
                var serviceResponse = await _roleService.GetRolesById(id);
                if (serviceResponse.success)
                {
                    return Ok(serviceResponse.getData());
                }
                else
                {
                    return BadRequest(serviceResponse.getMessage());
                }
            }
            catch
            {
                throw new HttpStatusException(500, "Something went wrong");
            }
        }

        // PUT: api/Roles/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateRoleAsync(int id, AddRoleDTO roleDTO)
        {
            try
            {
                var serviceResponse = await _roleService.UpdateRoleAsync(id, roleDTO);
                if (serviceResponse.success)
                {
                    return Ok(serviceResponse.getMessage());
                }
                else
                {
                    return BadRequest(serviceResponse.getMessage());
                }
            }
            catch
            {
                throw new HttpStatusException(500, "Something went wrong");
            }
        }

        // POST: api/Roles
        [HttpPost]
        public async Task<IActionResult> AddNewRole([FromBody] AddRoleDTO roleDTO)
        {
            try
            {
                var serviceResponse = await _roleService.AddRoleAsync(roleDTO);
                if (ModelState.IsValid)
                {
                    if (serviceResponse.success)
                    {
                        return Ok(serviceResponse.getMessage());
                    }
                    else
                    {
                        return BadRequest(serviceResponse.getMessage());
                    }
                }
                else
                {
                    return BadRequest("Invalid role data.");
                }
            }
            catch
            {
                throw new HttpStatusException(500, "Something went wrong");
            }
        }


        // DELETE: api/Roles/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRole(int id)
        {
            try
            {
                var serviceResponse = await _roleService.DeleteRoleAsync(id);
                if (serviceResponse.success)
                {
                    return Ok(serviceResponse.getMessage());
                }
                else
                {
                    return BadRequest(serviceResponse.getMessage());
                }
            }
            catch
            {
                throw new HttpStatusException(500, "Something went wrong");
            }
        }

        private bool RoleExists(int id)
        {
            return _context.Roles.Any(e => e.Id == id);
        }
    }
}
