using AutoMapper;
using TestWebAPI.DTOs.Auth;
using TestWebAPI.DTOs.Category;
using TestWebAPI.DTOs.ChatHub;
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
                 }).ToList())).ReverseMap(); 
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
                    id = p.id,
                    title = p.title,
                    description = p.description,
                    avatar = p.avatar,
                    price = p.price,
                    categoryId = p.categoryId,
                    status = p.status,
                    createdAt = p.createdAt,
                    updatedAt = p.updatedAt,
                    dataDetail = p.PropertyHasDetail != null ? new GetPropertyHasDetailDTO
                    {
                        id = p.PropertyHasDetail.id,
                        sellerId = p.PropertyHasDetail.sellerId,
                        propertyId = p.PropertyHasDetail.propertyId,
                        province = p.PropertyHasDetail.province,
                        city = p.PropertyHasDetail.city,
                        images = p.PropertyHasDetail.images,
                        address = p.PropertyHasDetail.address,
                        bedroom = p.PropertyHasDetail.bedroom,
                        bathroom = p.PropertyHasDetail.bathroom,
                        yearBuild = p.PropertyHasDetail.yearBuild,
                        size = p.PropertyHasDetail.size,
                        type = p.PropertyHasDetail.type
                    } : null
                }).ToList()))
                    .ReverseMap();

            CreateMap<Category, CategoryDTO>().ReverseMap();

            //property
            CreateMap<Property, GetPropertyDTO>()
                .ForMember(dest => dest.dataDetail, opt => opt.MapFrom(src => src.PropertyHasDetail != null ? new GetPropertyHasDetailDTO
                {
                    id = src.PropertyHasDetail.id,
                    sellerId = src.PropertyHasDetail.sellerId,
                    propertyId = src.PropertyHasDetail.propertyId,
                    province = src.PropertyHasDetail.province,
                    city = src.PropertyHasDetail.city,
                    images = src.PropertyHasDetail.images,
                    address = src.PropertyHasDetail.address,
                    bedroom = src.PropertyHasDetail.bedroom,
                    bathroom = src.PropertyHasDetail.bathroom,
                    yearBuild = src.PropertyHasDetail.yearBuild,
                    size = src.PropertyHasDetail.size,
                    type = src.PropertyHasDetail.type
                } : null))
                .ReverseMap();

            CreateMap<Property, PropertyDTO>().ReverseMap();
            CreateMap<Property, GetPropertyDTO>().ReverseMap();
            CreateMap<PropertyHasDetail, PropertyHasDetailDTO>().ReverseMap();
            CreateMap<PropertyHasDetail, GetPropertyHasDetailDTO>().ReverseMap();

            // ChatHub
            CreateMap<Conversation, ConversationDTO>().ReverseMap();
            CreateMap<Message, MessageDTO>().ReverseMap();
            CreateMap<Message, GetMessageDTO>();

        }
    }
}
