using AutoMapper;
using TestWebAPI.DTOs.Appointment;
using TestWebAPI.DTOs.Auth;
using TestWebAPI.DTOs.Category;
using TestWebAPI.DTOs.ChatHub;
using TestWebAPI.DTOs.Contract;
using TestWebAPI.DTOs.Evaluate;
using TestWebAPI.DTOs.JWT;
using TestWebAPI.DTOs.Notification;
using TestWebAPI.DTOs.Payment;
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
            CreateMap<User, GetUserDTO>()
                .ForMember(dest => dest.dataMedia, opt => opt.MapFrom(src => src.User_Medias.Select(um => new GetUserMediaDTO
                {
                    id = um.id,
                    icon = um.icon,
                    link = um.link,
                    provider = um.provider,
                }).ToList())).ReverseMap();

            CreateMap<User_Media, GetUserMediaDTO>();
            CreateMap<User, UserDTO>().ReverseMap();
            CreateMap<User, AvatarUserDTO>().ReverseMap();
            CreateMap<User, EmailUSerDTO>().ReverseMap();

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
                    type = p.type,
                    createdAt = p.createdAt,
                    updatedAt = p.updatedAt,
                    dataDetail = p.PropertyHasDetail != null ? new GetPropertyHasDetailDTO
                    {
                        id = p.PropertyHasDetail.id,
                        userData = p.PropertyHasDetail.seller != null ? new GetUserDTO
                        {
                            id = p.PropertyHasDetail.seller.id,
                            first_name = p.PropertyHasDetail.seller.first_name,
                            last_name = p.PropertyHasDetail.seller.last_name,
                            avatar = p.PropertyHasDetail.seller.avatar,
                            phone = p.PropertyHasDetail.seller.phone,
                            email = p.PropertyHasDetail.seller.email
                        } : null,
                        propertyId = p.PropertyHasDetail.propertyId,
                        province = p.PropertyHasDetail.province,
                        city = p.PropertyHasDetail.city,
                        images = p.PropertyHasDetail.images,
                        address = p.PropertyHasDetail.address,
                        bedroom = p.PropertyHasDetail.bedroom,
                        bathroom = p.PropertyHasDetail.bathroom,
                        yearBuild = p.PropertyHasDetail.yearBuild,
                        size = p.PropertyHasDetail.size,
                        status = p.PropertyHasDetail.status
                    } : null
                }).ToList()))
                    .ReverseMap();

            CreateMap<Category, CategoryDTO>().ReverseMap();

            //property
            CreateMap<Property, GetPropertyDTO>()
                .ForMember(dest => dest.dataDetail, opt => opt.MapFrom(src => src.PropertyHasDetail != null ? new GetPropertyHasDetailDTO
                {
                    id = src.PropertyHasDetail.id,
                    userData = src.PropertyHasDetail.seller != null ? new GetUserDTO
                    {
                        id = src.PropertyHasDetail.seller.id,
                        first_name = src.PropertyHasDetail.seller.first_name,
                        last_name = src.PropertyHasDetail.seller.last_name,
                        avatar = src.PropertyHasDetail.seller.avatar,
                        phone = src.PropertyHasDetail.seller.phone,
                        email = src.PropertyHasDetail.seller.email
                    } : null,
                    propertyId = src.PropertyHasDetail.propertyId,
                    province = src.PropertyHasDetail.province,
                    city = src.PropertyHasDetail.city,
                    images = src.PropertyHasDetail.images,
                    address = src.PropertyHasDetail.address,
                    bedroom = src.PropertyHasDetail.bedroom,
                    bathroom = src.PropertyHasDetail.bathroom,
                    yearBuild = src.PropertyHasDetail.yearBuild,
                    size = src.PropertyHasDetail.size,
                    status = src.PropertyHasDetail.status
                } : null))
                .ReverseMap();
            CreateMap<Property, PropertyDTO>().ReverseMap();
            CreateMap<PropertyHasDetail, PropertyHasDetailDTO>().ReverseMap();
            CreateMap<PropertyHasDetail, GetPropertyHasDetailDTO>().ReverseMap();

            // Realtime
            CreateMap<Conversation, GetConversationDTO>()
                .ForMember(dest => dest.dataMessages, opt => opt.MapFrom(src => src.Messages.Select(m => new GetMessageDTO
                {
                    conversationId = m.conversationId,
                    userId = m.userId,
                    content = m.content,
                    createdAt = m.createdAt,
                }).ToList()))
                .ReverseMap()
                .ForMember(dest => dest.Messages, opt => opt.Ignore());

            CreateMap<Conversation, ConversationDTO>().ReverseMap();

            CreateMap<Message, MessageDTO>().ReverseMap();
            CreateMap<Message, GetMessageDTO>().ReverseMap();

            CreateMap<Notification, NotificationDTO>().ReverseMap();
            CreateMap<Notification, GetNotificationDTO>().ForMember(dest => dest.dataUser, opt => opt.MapFrom(src => src.user != null ? new GetUserDTO
            {
                id = src.user.id,
                avatar = src.user.avatar,
                first_name = src.user.first_name,
                last_name = src.user.last_name,
            } : null)).ReverseMap();

            //Appointment
            CreateMap<Appointment, AppointmentDTO>().ReverseMap();
            CreateMap<Appointment, GetAppointmentDTO>().ReverseMap();

            //Contract
            CreateMap<Contract, ContractDTO>().ReverseMap();
            CreateMap<Contract, GetContractDTO>().ReverseMap();

            //Payment
            CreateMap<Payment, PaymentDTO>().ReverseMap();
            CreateMap<Payment, GetPaymentDTO>().ReverseMap();

            //Payment
            CreateMap<Evaluate, EvaluateDTO>().ReverseMap();
            CreateMap<Evaluate, GetEvaluateDTO>().ReverseMap();
        }
    }
}
