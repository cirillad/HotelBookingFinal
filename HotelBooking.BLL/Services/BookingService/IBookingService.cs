using HotelBooking.BLL.DTOs.BookingDto;
using HotelBooking.DAL.Entities;
using HotelBooking.BLL.Services;

namespace HotelBooking.BLL.Services.BookingService
{
    public interface IBookingService
    {
        Task<ServiceResponse<BookingDto>> CreateBookingAsync(BookingDto bookingDto);
        Task<ServiceResponse<BookingDto>> GetBookingByIdAsync(int bookingId);
        Task<ServiceResponse<List<BookingDto>>> GetUserBookingsAsync(string userId);
        Task<ServiceResponse<List<BookingDto>>> GetAllBookingsAsync();
        Task<ServiceResponse<bool>> CancelBookingAsync(int bookingId);
    }
}
