using AutoMapper;
using TestWebAPI.Repositories.Interfaces;
using TestWebAPI.Services.Interfaces;
using TestWebAPI.Response;
using TestWebAPI.DTOs.Permisstion;
using TestWebAPI.DTOs.Role;
using static TestWebAPI.Response.HttpStatus;
using TestWebAPI.Models;
using TestWebAPI.Helpers;

namespace TestWebAPI.Services
{
    public class PermissionServices : IPermissionServices
    {
        private IPermisstionRepositories _permisstionRepo;
        private IMapper _mapper;

        public PermissionServices(IPermisstionRepositories permisstionRepo, IMapper mapper) {
            _permisstionRepo = permisstionRepo;
            _mapper = mapper;
        }

        public async Task<ServiceResponse<AddPermissionDTO>> CreatePermissionAsyn(AddPermissionDTO permisstionDTO)
        {
            var serviceResponse = new ServiceResponse<AddPermissionDTO>();
            try
            {
                var existsPermission = await _permisstionRepo.GetPermissionByValueAsyn(permisstionDTO.value);
                if (existsPermission != null)
                {
                    serviceResponse.statusCode = EHttpType.Conflict;
                    serviceResponse.success = false;
                    serviceResponse.message = "Permission already exists.";
                    return serviceResponse;
                }
                var permission = _mapper.Map<Permission>(permisstionDTO);
                permission.code = CodeGenerator.GenerateCode(permisstionDTO.value);
                var addPermission = await _permisstionRepo.CreatePermissionAsyn(permission);
                serviceResponse.statusCode = EHttpType.Success;
                serviceResponse.success = true;
                serviceResponse.message = "Permission added successfully.";
            }
            catch (Exception ex)
            {
                serviceResponse.success = false;
                serviceResponse.message = $"An unexpected error occurred: {ex.Message}";
                serviceResponse.statusCode = EHttpType.InternalError;
            }
            return serviceResponse;
        }

        public async Task<ServiceResponse<PermisstionDTO>> UpdatePermissionAsyn(int id, PermisstionDTO permisstionDTO)
        {
            var serviceResponse = new ServiceResponse<PermisstionDTO>();
            try
            {
                var oldPermission = await _permisstionRepo.GetPermissionByIdAsyn(id);
                if (oldPermission == null)
                {
                    serviceResponse.success = false;
                    serviceResponse.message = "Permission not found.";
                    serviceResponse.statusCode = EHttpType.NotFound;
                    return serviceResponse;
                }
                var existsPermission = await _permisstionRepo.GetPermissionByValueAsyn(permisstionDTO.value);
                if (existsPermission != null)
                {
                    serviceResponse.statusCode = EHttpType.Conflict;
                    serviceResponse.success = false;
                    serviceResponse.message = "Permission already exists.";
                    return serviceResponse;
                }
                var permission = _mapper.Map<Permission>(permisstionDTO);
                permission.code = CodeGenerator.GenerateCode(permisstionDTO.value);
                var updatePermission = await _permisstionRepo.UpdatePermissionAsyn(oldPermission, permission);
                serviceResponse.success = true;
                serviceResponse.statusCode = EHttpType.Success;
                serviceResponse.message = "Permission updated successfully.";
            }
            catch (Exception ex)
            {
                serviceResponse.success = false;
                serviceResponse.message = $"An unexpected error occurred: {ex.Message}";
                serviceResponse.statusCode = EHttpType.InternalError;
            }
            return serviceResponse;
        }

        public async Task<ServiceResponse<PermisstionDTO>> DeletePermissionAsyn(int id)
        {
            var serviceResponse = new ServiceResponse<PermisstionDTO>();
            try
            {
                var existsPermission = await _permisstionRepo.GetPermissionByIdAsyn(id);
                if (existsPermission == null)
                {
                    serviceResponse.statusCode = EHttpType.NotFound;
                    serviceResponse.success = false;
                    serviceResponse.message = "Permission don't exists.";
                    return serviceResponse;
                }
                await _permisstionRepo.DeletePermissionAsyn(existsPermission);
                serviceResponse.success = true;
                serviceResponse.message = "Permission delete successfully.";
                serviceResponse.statusCode = EHttpType.Success;
            }
            catch (Exception ex)
            {
                serviceResponse.success = false;
                serviceResponse.message = $"An unexpected error occurred: {ex.Message}";
                serviceResponse.statusCode = EHttpType.InternalError;
            }
            return serviceResponse;
        }
        public async Task<ServiceResponse<List<PermisstionDTO>>> GetAllPermissionAsyn()
        {
            var serviceResponse = new ServiceResponse<List<PermisstionDTO>>();
            try
            {
                var permission = await _permisstionRepo.GetAllPermissionAsyn();
                if (permission == null)
                {
                    serviceResponse.success = false;
                    serviceResponse.statusCode = EHttpType.NotFound;
                    serviceResponse.message = "No roles found in the database.";
                    return serviceResponse;
                }
                serviceResponse.data = _mapper.Map<List<PermisstionDTO>>(permission);
                serviceResponse.success = true;
                serviceResponse.message = "Permissions get all successfully.";
                serviceResponse.statusCode = EHttpType.Success;
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
