using AutoMapper;
using HotelBooking.BLL.DTOs.RoomDto;
using HotelBooking.DAL.Entities;

namespace HotelBooking_API.MapperProfiles
{
    public class RoomProfile : Profile
    {
        public RoomProfile()
        {
            // Маппінг між Room та RoomDto
            CreateMap<Room, RoomDto>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.RoomNumber, opt => opt.MapFrom(src => src.Number))
                .ForMember(dest => dest.Type, opt => opt.MapFrom(src => src.Type))
                .ForMember(dest => dest.Price, opt => opt.MapFrom(src => src.Price))
                .ForMember(dest => dest.IsAvailable, opt => opt.MapFrom(src => src.IsAvailable)); // Додав маппінг для IsAvailable

            // Маппінг між RoomDto та Room
            CreateMap<RoomDto, Room>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Number, opt => opt.MapFrom(src => src.RoomNumber))
                .ForMember(dest => dest.Type, opt => opt.MapFrom(src => src.Type))
                .ForMember(dest => dest.Price, opt => opt.MapFrom(src => src.Price))
                .ForMember(dest => dest.IsAvailable, opt => opt.MapFrom(src => src.IsAvailable)); // Додав маппінг для IsAvailable
        }
    }
}
