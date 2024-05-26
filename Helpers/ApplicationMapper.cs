using AutoMapper;
using TestWebAPI.DTOs.Role;
using TestWebAPI.Models;

namespace TestWebAPI.Helpers
{
    public class ApplicationMapper : Profile

    {
        public ApplicationMapper() {
            CreateMap<Role, RoleDTO>().ReverseMap();
            CreateMap<Role, AddRoleDTO>().ReverseMap();

        }
    }
}
