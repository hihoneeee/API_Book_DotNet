using AutoMapper;
using TestWebAPI.DTOs.Auth;
using TestWebAPI.DTOs.JWT;
using TestWebAPI.DTOs.Permisstion;
using TestWebAPI.DTOs.Role;
using TestWebAPI.DTOs.RoleHasPermission;
using TestWebAPI.DTOs.User;
using TestWebAPI.Models;

namespace TestWebAPI.Helpers
{
    public class ApplicationMapper : Profile

    {
        public ApplicationMapper() {
            //role
            CreateMap<Role, RoleDTO>()
                 .ForMember(dest => dest.dataPermission, opt => opt.MapFrom(src => src.Role_Permissions.Select(rp => new PermisstionDTO
                 {
                     code = rp.Permission.code,
                     value = rp.Permission.value
                 }).ToList())); CreateMap<Role, AddRoleDTO>().ReverseMap();

            // auth
            CreateMap<User, AuthRegisterDTO>().ReverseMap();
            CreateMap<User, AuthLoginDTO>().ReverseMap();
            CreateMap<User, AuthResetPasswordDTO>().ReverseMap();

            //user
            CreateMap<User, UserDTO>().ReverseMap();

            //JWT
            CreateMap<JWT, jwtDTO>().ReverseMap();

            //Permisstion
            CreateMap<Permission, PermisstionDTO>().ReverseMap();

            //role has permission
            CreateMap<Role_Permission, RoleHasPermissionDTO>().ReverseMap();
        }
    }
}
