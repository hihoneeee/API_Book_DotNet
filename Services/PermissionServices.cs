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
                    serviceResponse.SetExisting("Permission");
                    return serviceResponse;
                }
                var permission = _mapper.Map<Permission>(permisstionDTO);
                permission.code = CodeGenerator.GenerateCode(permisstionDTO.value);
                var addPermission = await _permisstionRepo.CreatePermissionAsyn(permission);
                serviceResponse.SetSuccess("Permission added successfully!");
            }
            catch (Exception ex)
            {
                serviceResponse.SetError(ex.Message);
            }
            return serviceResponse;
        }

        public async Task<ServiceResponse<PermisstionDTO>> UpdatePermissionAsyn(int id, AddPermissionDTO permisstionDTO)
        {
            var serviceResponse = new ServiceResponse<PermisstionDTO>();
            try
            {
                var oldPermission = await _permisstionRepo.GetPermissionByIdAsyn(id);
                if (oldPermission == null)
                {
                    serviceResponse.SetNotFound("Permission not found!");
                    return serviceResponse;
                }
                var existsPermission = await _permisstionRepo.GetPermissionByValueAsyn(permisstionDTO.value);
                if (existsPermission != null)
                {
                    serviceResponse.SetExisting("Permission already exists!");
                    return serviceResponse;
                }
                var permission = _mapper.Map<Permission>(permisstionDTO);
                permission.code = CodeGenerator.GenerateCode(permisstionDTO.value);
                var updatePermission = await _permisstionRepo.UpdatePermissionAsyn(oldPermission, permission);
                serviceResponse.SetSuccess("Permission updated successfully!");
            }
            catch (Exception ex)
            {
                serviceResponse.SetError(ex.Message);
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
                    serviceResponse.SetNotFound("Permission not found!");
                    return serviceResponse;
                }
                await _permisstionRepo.DeletePermissionAsyn(existsPermission);
                serviceResponse.SetSuccess("Permission delete successfully!");

            }
            catch (Exception ex)
            {
                serviceResponse.SetError(ex.Message);
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
                    serviceResponse.SetNotFound("Permission not found!");
                    return serviceResponse;
                }
                serviceResponse.data = _mapper.Map<List<PermisstionDTO>>(permission);
                serviceResponse.SetSuccess("Permission get all successfully!");
            }
            catch (Exception ex)
            {
                serviceResponse.SetError(ex.Message);
            }
            return serviceResponse;
        }
    }
}
