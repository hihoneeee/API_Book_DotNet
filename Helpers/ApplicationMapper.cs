using AutoMapper;
using TestWebAPI.DTOs.Auth;
using TestWebAPI.DTOs.JWT;
using TestWebAPI.DTOs.Permisstion;
using TestWebAPI.DTOs.Role;
using TestWebAPI.DTOs.User;
using TestWebAPI.Models;

namespace TestWebAPI.Helpers
{
    public class ApplicationMapper : Profile

    {
        public ApplicationMapper() {
            //role
            CreateMap<Role, RoleDTO>().ReverseMap();
            CreateMap<Role, AddRoleDTO>().ReverseMap();

            // auth
            CreateMap<User, AuthRegisterDTO>().ReverseMap();
            CreateMap<User, AuthLoginDTO>().ReverseMap();
            CreateMap<User, AuthChangePasswordDTO>().ReverseMap();

            //user
            CreateMap<User, UserDTO>().ReverseMap();

            //JWT
            CreateMap<JWT, jwtDTO>().ReverseMap();

            //Permisstion
            CreateMap<Permission, PermisstionDTO>().ReverseMap();
        }
    }
}
