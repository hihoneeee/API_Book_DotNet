using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System.Data;
using TestWebAPI.DTOs.Role;
using TestWebAPI.DTOs.RoleHasPermission;
using TestWebAPI.Helpers;
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
        private readonly IPermisstionRepositories _permissionRepo;

        public RoleHasPermissionServices(IRoleHasPermissionRepositories roleHasPermissionRepo, IMapper mapper, IRoleRepositories roleRepo, IPermisstionRepositories permissionRepo)
        {
            _roleHasPermissionRepo = roleHasPermissionRepo;
            _mapper = mapper;
            _roleRepo = roleRepo;
            _permissionRepo = permissionRepo;
        }

        public async Task<ServiceResponse<AddRoleHasPermissionDTO>> AssignPermissionsAsync(AddRoleHasPermissionDTO addRoleHasPermissionDTO)
        {
            var serviceResponse = new ServiceResponse<AddRoleHasPermissionDTO>();
            try
            {
                var existingRole = await _roleRepo.GetRoleByCodeAsyn(addRoleHasPermissionDTO.codeRole);
                if (existingRole == null)
                {
                    serviceResponse.SetNotFound("Role");
                    return serviceResponse;
                }

                var currentPermissions = await _roleHasPermissionRepo.GetPermissionsByRoleCodeAsync(addRoleHasPermissionDTO.codeRole);
                var currentPermissionCodes = currentPermissions.Select(p => p.codePermission).ToList();

                // Remove unselected permissions
                foreach (var permission in currentPermissions)
                {
                    if (!addRoleHasPermissionDTO.codePermission.Contains(permission.codePermission))
                    {
                        await _roleHasPermissionRepo.RemovePermissionAsync(permission);
                    }
                }

                // Add new permissions
                foreach (var permissionCode in addRoleHasPermissionDTO.codePermission)
                {
                    if (!currentPermissionCodes.Contains(permissionCode))
                    {
                        var existingPermission = await _permissionRepo.GetPermissionByCodeAsyn(permissionCode);
                        if (existingPermission == null)
                        {
                            serviceResponse.SetNotFound($"Permission {permissionCode}");
                            return serviceResponse;
                        }

                        var rolePermission = new Role_Permission
                        {
                            codeRole = addRoleHasPermissionDTO.codeRole,
                            codePermission = permissionCode
                        };

                        await _roleHasPermissionRepo.AssignPermissionAsync(rolePermission);
                    }
                }

                serviceResponse.SetSuccess("Permissions assigned successfully!");
            }
            catch (Exception ex)
            {
                serviceResponse.SetError(ex.Message);
            }
            return serviceResponse;
        }
    }
}
