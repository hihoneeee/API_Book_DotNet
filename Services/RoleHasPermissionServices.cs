using AutoMapper;
using System.Data;
using TestWebAPI.DTOs.Role;
using TestWebAPI.DTOs.RoleHasPermission;
using TestWebAPI.Models;
using TestWebAPI.Repositories.Interfaces;
using TestWebAPI.Response;
using TestWebAPI.Services.Interfaces;
using static TestWebAPI.Response.HttpStatus;

namespace TestWebAPI.Services
{
    public class RoleHasPermissionServices : IRoleHasPermissionServices
    {
        private IRoleHasPermissionRepositories _roleHasPermissionRepo;
        private IMapper _mapper;
        private readonly IRoleRepositories _roleRepo;
        private readonly IPermisstionRepositories _permisstionRepo;

        public RoleHasPermissionServices(IRoleHasPermissionRepositories roleHasPermissionRepo, IMapper mapper, IRoleRepositories roleRepo, IPermisstionRepositories permisstionRepo)
        {
            _roleHasPermissionRepo = roleHasPermissionRepo;
            _mapper = mapper;
            _roleRepo = roleRepo;
            _permisstionRepo = permisstionRepo;
        }
 
        public async Task<ServiceResponse<RoleHasPermissionDTO>> AssignPermissionAsyn(string roleCode, string permissionCode, RoleHasPermissionDTO roleHasPermissionDTO)
        {
            var serviceResponse = new ServiceResponse<RoleHasPermissionDTO>();
            try
            {
                var existingRole = await _roleRepo.GetRoleByCodeAsyn(roleCode);
                if(existingRole == null)
                {
                    serviceResponse.statusCode = EHttpType.NotFound;
                    serviceResponse.success = false;
                    serviceResponse.message = "Role not existing!";
                    return serviceResponse;
                }
                var existingPermission = await _permisstionRepo.GetPermissionByCodeAsyn(permissionCode);
                if (existingRole == null)
                {
                    serviceResponse.statusCode = EHttpType.NotFound;
                    serviceResponse.success = false;
                    serviceResponse.message = "Permission not existing!";
                    return serviceResponse;
                }
                var checkHasPermission = await _roleHasPermissionRepo.CheckRoleHasPermissonAsync(roleCode, permissionCode);
                if (checkHasPermission)
                {
                    serviceResponse.statusCode = EHttpType.CannotCreate;
                    serviceResponse.success = false;
                    serviceResponse.message = "The role already has this permission!";
                    return serviceResponse;
                }
                var create = _mapper.Map<Role_Permission>(roleHasPermissionDTO);
                create.codePermission = permissionCode;
                create.codeRole = roleCode;
                var assignPermission = await _roleHasPermissionRepo.AssignPermissionAsync(create);
                serviceResponse.statusCode = EHttpType.Success;
                serviceResponse.success = true;
                serviceResponse.message = "Permission assigned successfully.";

            }
            catch (Exception ex)
            {
                serviceResponse.success = false;
                serviceResponse.message = $"An unexpected error occurred: {ex.Message}";
                serviceResponse.statusCode = EHttpType.InternalError;
            }
            return serviceResponse;
        }

    }
}
