using HotelBooking.BLL.DTOs.BookingDto;
using HotelBooking.DAL.Entities;
using HotelBooking.DAL.Repositories.BookingRepos;
using HotelBooking.BLL.Services;
using Microsoft.EntityFrameworkCore;
using AutoMapper;

namespace HotelBooking.BLL.Services.BookingService
{
    public class BookingService : IBookingService
    {
        private readonly IBookingRepository _bookingRepository;
        private readonly IMapper _mapper;

        public BookingService(IBookingRepository bookingRepository, IMapper mapper)
        {
            _bookingRepository = bookingRepository;
            _mapper = mapper;
        }

        // Створення нового бронювання
        // Створення нового бронювання
        public async Task<ServiceResponse<BookingDto>> CreateBookingAsync(BookingDto bookingDto)
        {
            var response = new ServiceResponse<BookingDto>();

            try
            {
                // Перетворення BookingDto в Booking
                var booking = _mapper.Map<Booking>(bookingDto);

                // Додаємо нове бронювання в базу
                await _bookingRepository.CreateAsync(booking);

                // Перетворення Booking назад в BookingDto
                response.Data = _mapper.Map<BookingDto>(booking);
                response.Message = "Бронювання успішно створено!";
                response.IsSuccess = true;
            }
            catch (Exception ex)
            {
                response.Message = $"Помилка при створенні бронювання: {ex.Message}";
                response.IsSuccess = false;
            }

            return response;
        }

        // Отримати бронювання за ідентифікатором
        public async Task<ServiceResponse<BookingDto>> GetBookingByIdAsync(int bookingId)
        {
            var response = new ServiceResponse<BookingDto>();

            try
            {
                var booking = await _bookingRepository.GetByIdAsync(bookingId);

                if (booking == null)
                {
                    response.Message = "Бронювання не знайдено!";
                    response.IsSuccess = false;
                }
                else
                {
                    response.Data = _mapper.Map<BookingDto>(booking);
                    response.IsSuccess = true;
                }
            }
            catch (Exception ex)
            {
                response.Message = $"Помилка при отриманні бронювання: {ex.Message}";
                response.IsSuccess = false;
            }

            return response;
        }

        // Отримати всі бронювання користувача
        public async Task<ServiceResponse<List<BookingDto>>> GetUserBookingsAsync(string userId)
        {
            var response = new ServiceResponse<List<BookingDto>>();

            try
            {
                var bookings = await _bookingRepository.GetAll()
                    .Where(b => b.UserId == userId)
                    .ToListAsync();

                if (!bookings.Any())
                {
                    response.Message = "У користувача немає бронювань!";
                    response.IsSuccess = false;
                }
                else
                {
                    response.Data = _mapper.Map<List<BookingDto>>(bookings);
                    response.IsSuccess = true;
                }
            }
            catch (Exception ex)
            {
                response.Message = $"Помилка при отриманні бронювань: {ex.Message}";
                response.IsSuccess = false;
            }

            return response;
        }

        // Отримати всі бронювання
        public async Task<ServiceResponse<List<BookingDto>>> GetAllBookingsAsync()
        {
            var response = new ServiceResponse<List<BookingDto>>();

            try
            {
                var bookings = await _bookingRepository.GetAll()
                    .ToListAsync();

                if (!bookings.Any())
                {
                    response.Message = "Бронювань не знайдено!";
                    response.IsSuccess = false;
                }
                else
                {
                    response.Data = _mapper.Map<List<BookingDto>>(bookings);
                    response.IsSuccess = true;
                }
            }
            catch (Exception ex)
            {
                response.Message = $"Помилка при отриманні всіх бронювань: {ex.Message}";
                response.IsSuccess = false;
            }

            return response;
        }

        // Скасувати бронювання
        public async Task<ServiceResponse<bool>> CancelBookingAsync(int bookingId)
        {
            var response = new ServiceResponse<bool>();

            try
            {
                var booking = await _bookingRepository.GetByIdAsync(bookingId);

                if (booking == null)
                {
                    response.Message = "Бронювання не знайдено!";
                    response.IsSuccess = false;
                }
                else
                {
                    // Викликаємо метод для видалення бронювання
                    await _bookingRepository.DeleteAsync(booking);  // Використовуємо метод DeleteAsync замість UpdateAsync

                    response.Data = true;
                    response.Message = "Бронювання успішно видалено!";  // Повідомлення змінено на видалення
                    response.IsSuccess = true;
                }
            }
            catch (Exception ex)
            {
                response.Message = $"Помилка при скасуванні бронювання: {ex.Message}";
                response.IsSuccess = false;
            }

            return response;
        }


    }
}
