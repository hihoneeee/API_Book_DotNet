using AutoMapper;
using TestWebAPI.DTOs.Auth;
using TestWebAPI.DTOs.Category;
using TestWebAPI.DTOs.JWT;
using TestWebAPI.DTOs.Permisstion;
using TestWebAPI.DTOs.Property;
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
                 }).ToList())); 
            CreateMap<Role, AddRoleDTO>().ReverseMap();

            //auth
            CreateMap<User, AuthRegisterDTO>().ReverseMap();
            CreateMap<User, AuthLoginDTO>().ReverseMap();
            CreateMap<User, AuthForgotPasswordDTO>().ReverseMap();

            //user
            CreateMap<User, UserDTO>().ReverseMap();

            //JWT
            CreateMap<JWT, jwtDTO>().ReverseMap();

            //permisstion
            CreateMap<Permission, PermisstionDTO>().ReverseMap();
            CreateMap<Permission, AddPermissionDTO>().ReverseMap();

            //role has permission
            CreateMap<Role_Permission, RoleHasPermissionDTO>().ReverseMap();

            //category
            CreateMap<Category, GetCategoryDTO>()
                                .ForMember(dest => dest.dataProperties, opt => opt.MapFrom(src => src.Properties.Select(p => new GetPropertyDTO
                                 {
                                     title = p.title,
                                     description = p.description,
                                     avatar = p.avatar,
                                     price = p.price,
                                     categoryId = p.categoryId,
                                     sellerId = p.PropertyHasDetails.FirstOrDefault().sellerId,
                                 }).ToList()));
            CreateMap<Category, CategoryDTO>().ReverseMap();
            //property
            CreateMap<Property, GetPropertyDTO>().ReverseMap();
        }
    }
}
