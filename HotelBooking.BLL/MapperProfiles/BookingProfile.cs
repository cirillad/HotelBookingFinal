using AutoMapper;
using HotelBooking.BLL.DTOs.BookingDto;
using HotelBooking.DAL.Entities;

namespace HotelBooking_API.MapperProfiles
{
    public class BookingProfile : Profile
    {
        public BookingProfile()
        {
            // Маппінг між Booking та BookingDto
            CreateMap<Booking, BookingDto>()
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.User.FirstName + " " + src.User.LastName))  // Об'єднуємо ім'я та прізвище користувача
                .ForMember(dest => dest.RoomName, opt => opt.MapFrom(src => src.Room.Number));  // Наприклад, поле Name кімнати

            // Маппінг між BookingDto та Booking
            CreateMap<BookingDto, Booking>()
                .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.UserId))
                .ForMember(dest => dest.RoomId, opt => opt.MapFrom(src => src.RoomId))
                .ForMember(dest => dest.CheckIn, opt => opt.MapFrom(src => src.CheckIn))
                .ForMember(dest => dest.CheckOut, opt => opt.MapFrom(src => src.CheckOut))
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status));
        }
    }
}
