using AutoMapper;
using System.ComponentModel.DataAnnotations;
using System.Data;
using TestWebAPI.DTOs.Role;
using TestWebAPI.Helpers;
using TestWebAPI.Models;
using TestWebAPI.Repositories.Interfaces;
using TestWebAPI.Response;
using TestWebAPI.Services.Interfaces;
using static TestWebAPI.Response.HttpStatus;

namespace TestWebAPI.Services
{
    public class RoleServices : IRoleService
    {
        private readonly IRoleRepositories _roleRepo;
        private readonly IMapper _mapper;

        public RoleServices(IRoleRepositories roleRepo, IMapper mapper) {
            _roleRepo = roleRepo;
            _mapper = mapper;
        }
        public async Task<ServiceResponse<AddRoleDTO>> AddRoleAsync(AddRoleDTO roleDTO)
        {
            var serviceResponse = new ServiceResponse<AddRoleDTO>();
            try
            {
                var existingRole = await _roleRepo.GetRoleByValueAsync(roleDTO.value);
                if (existingRole != null)
                {
                    serviceResponse.statusCode = EHttpType.Conflict;
                    serviceResponse.success = false;
                    serviceResponse.message = "Role already exists.";
                    return serviceResponse;
                }
                var role = _mapper.Map<Role>(roleDTO);
                role.code = CodeGenerator.GenerateCode(roleDTO.value);
                var adddRole = await _roleRepo.AddRoleAsync(role);
                serviceResponse.statusCode = EHttpType.Success;
                serviceResponse.message = "Role added successfully.";
                serviceResponse.success = true;
            }
            catch (Exception ex)
            {
                serviceResponse.SetError(ex.Message);
            }
            return serviceResponse;
        }

        public async Task<ServiceResponse<List<RoleDTO>>> GetAllRoles()
        {
            var serviceResponse = new ServiceResponse<List<RoleDTO>>();
            try
            {
                var roles = await _roleRepo.GetAllRoles();
                if (roles == null)
                {
                    serviceResponse.success = false;
                    serviceResponse.statusCode = EHttpType.NotFound;
                    serviceResponse.message = "No roles found in the database.";
                    return serviceResponse;
                }
                serviceResponse.data = _mapper.Map<List<RoleDTO>>(roles);
                serviceResponse.SetSuccess("Get successfully!");
            }
            catch (Exception ex)
            {
                serviceResponse.SetError(ex.Message);
            }
            return serviceResponse;

        }

        public async Task<ServiceResponse<RoleDTO>> GetRolesById(int id)
        {
            var serviceResponse = new ServiceResponse<RoleDTO>();
            try
            {
                var role = await _roleRepo.GetRolesById(id);
                if (role == null)
                {
                    serviceResponse.success = false;
                    serviceResponse.statusCode = EHttpType.NotFound;
                    serviceResponse.message = "No role found in the database.";
                    return serviceResponse;
                }
                serviceResponse.data = _mapper.Map<RoleDTO>(role);
                serviceResponse.SetSuccess("Get successfully!");
            }
            catch (Exception ex)
            {
                serviceResponse.SetError(ex.Message);
            }
            return serviceResponse;
        }

        public async Task<ServiceResponse<RoleDTO>> UpdateRoleAsync(int id, AddRoleDTO roleDTO)
        {
            var serviceResponse = new ServiceResponse<RoleDTO>();
            try
            {
                var oldRole = await _roleRepo.GetRolesById(id);
                if(oldRole == null)
                {
                    serviceResponse.SetNotFound("Role");
                    return serviceResponse;
                }
                var existingRole = await _roleRepo.GetRoleByValueAsync(roleDTO.value);
                if (existingRole != null)
                {
                    serviceResponse.SetExisting("Role");
                    return serviceResponse;
                }
                var role = _mapper.Map<Role>(roleDTO);
                role.code = CodeGenerator.GenerateCode(roleDTO.value);
                var updatedRole = await _roleRepo.UpdateRoleAsync(oldRole, role);
                serviceResponse.SetSuccess("Role updated successfully!");
            }
            catch (Exception ex)
            {
                serviceResponse.SetError(ex.Message);
            }
            return serviceResponse;
        }
        public async Task<ServiceResponse<RoleDTO>> DeleteRoleAsync(int id)
        {
            var serviceResponse = new ServiceResponse<RoleDTO>();
            try
            {
                var deletedRole = await _roleRepo.GetRolesById(id);
                if (deletedRole == null)
                {
                    serviceResponse.success = false;
                    serviceResponse.message = "Role not found.";
                    serviceResponse.statusCode = EHttpType.NotFound;
                    return serviceResponse;
                }
                await _roleRepo.DeleteRoleAsync(deletedRole);
                serviceResponse.SetSuccess("Role deleted successfully!");
            }
            catch (Exception ex)
            {
                serviceResponse.SetError(ex.Message);
            }
            return serviceResponse;
        }
    }
}
