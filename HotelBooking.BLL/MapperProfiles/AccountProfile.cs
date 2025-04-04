using AutoMapper;
using HotelBooking.BLL.DTOs.Account;
using HotelBooking.BLL.DTOs.UserDto;
using HotelBooking.DAL.Entities;

namespace HotelBooking.BLL.MapperProfiles
{
    public class AccountProfile : Profile
    {
        public AccountProfile()
        {
            // Мапінг між RegisterDto та AppUser
            CreateMap<RegisterDto, AppUser>()
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.UserName))
                .ForMember(dest => dest.FirstName, opt => opt.MapFrom(src => src.FirstName))
                .ForMember(dest => dest.LastName, opt => opt.MapFrom(src => src.LastName))
                .ForMember(dest => dest.PasswordHash, opt => opt.Ignore()) // Пароль хешується окремо
                .ForMember(dest => dest.UserRoles, opt => opt.Ignore()); // Ролі будуть призначені пізніше

            // Мапінг між LoginDto та AppUser
            CreateMap<LoginDto, AppUser>()
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.UserName))
                .ForMember(dest => dest.PasswordHash, opt => opt.MapFrom(src => src.Password)); // Пароль хешується окремо

            // Мапінг між AppUser та UserDto
            CreateMap<AppUser, UserDto>()
                .ForMember(dest => dest.Role, opt => opt.MapFrom(src => src.Role))  // Призначення ролі
                .ForMember(dest => dest.CreatedDate, opt => opt.MapFrom(src => src.CreatedDate))
                .ForMember(dest => dest.UpdatedDate, opt => opt.MapFrom(src => src.UpdatedDate));

            // Мапінг між AppRole та RoleDto
            CreateMap<AppRole, RoleDto>()
                .ForMember(dest => dest.RoleName, opt => opt.MapFrom(src => src.Name)); // Призначення назви ролі
        }
    }
}
