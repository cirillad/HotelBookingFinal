using AutoMapper;
using HotelBooking.BLL.DTOs.RoomDto;
using HotelBooking.DAL.Entities;
using HotelBooking.DAL.Repositories.RoomRepos;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HotelBooking.BLL.Services.RoomService
{
    public class RoomService : IRoomService
    {
        private readonly IRoomRepository _roomRepository;
        private readonly IMapper _mapper;

        // Конструктор для ін'єкції RoomRepository та IMapper
        public RoomService(IRoomRepository roomRepository, IMapper mapper)
        {
            _roomRepository = roomRepository;
            _mapper = mapper;
        }

        // Додавання нової кімнати
        public async Task<ServiceResponse<RoomDto>> AddRoomAsync(RoomDto roomDto)
        {
            var response = new ServiceResponse<RoomDto>();

            try
            {
                // Маппінг RoomDto на Room (використовуючи _mapper)
                var newRoom = _mapper.Map<Room>(roomDto);

                var addedRoom = await _roomRepository.CreateAsync(newRoom); // Використовуємо метод CreateAsync репозиторію для додавання

                response.Data = _mapper.Map<RoomDto>(addedRoom); // Маппінг з Room назад на RoomDto
                response.Message = "Кімната успішно додана!";
                response.IsSuccess = true;
            }
            catch (Exception ex)
            {
                response.Message = $"Помилка при додаванні кімнати: {ex.Message}";
                response.IsSuccess = false;
            }

            return response;
        }

        // Отримання кімнати за ID
        public async Task<ServiceResponse<RoomDto>> GetRoomByIdAsync(int roomId)
        {
            var response = new ServiceResponse<RoomDto>();

            try
            {
                var room = await _roomRepository.GetByIdAsync(roomId); // Використовуємо метод GetByIdAsync репозиторію для отримання кімнати

                if (room == null)
                {
                    response.Message = "Кімнату не знайдено!";
                    response.IsSuccess = false;
                }
                else
                {
                    response.Data = _mapper.Map<RoomDto>(room); // Маппінг Room на RoomDto
                    response.IsSuccess = true;
                }
            }
            catch (Exception ex)
            {
                response.Message = $"Помилка при отриманні кімнати: {ex.Message}";
                response.IsSuccess = false;
            }

            return response;
        }

        // Оновлення інформації про кімнату
        public async Task<ServiceResponse<RoomDto>> UpdateRoomAsync(int roomId, RoomDto roomDto)
        {
            var response = new ServiceResponse<RoomDto>();

            try
            {
                var room = await _roomRepository.GetByIdAsync(roomId); // Використовуємо метод GetByIdAsync репозиторію

                if (room == null)
                {
                    response.Message = "Кімнату не знайдено!";
                    response.IsSuccess = false;
                }
                else
                {
                    // Маппінг RoomDto на Room для оновлення
                    _mapper.Map(roomDto, room);

                    var updatedRoom = await _roomRepository.UpdateAsync(room); // Використовуємо метод UpdateAsync репозиторію для оновлення

                    response.Data = _mapper.Map<RoomDto>(updatedRoom); // Маппінг з Room назад на RoomDto
                    response.Message = "Кімната успішно оновлена!";
                    response.IsSuccess = true;
                }
            }
            catch (Exception ex)
            {
                response.Message = $"Помилка при оновленні кімнати: {ex.Message}";
                response.IsSuccess = false;
            }

            return response;
        }

        // Видалення кімнати
        public async Task<ServiceResponse<bool>> DeleteRoomAsync(int roomId)
        {
            var response = new ServiceResponse<bool>();

            try
            {
                var room = await _roomRepository.GetByIdAsync(roomId); // Використовуємо метод GetByIdAsync репозиторію для пошуку кімнати

                if (room == null)
                {
                    response.Message = "Кімнату не знайдено!";
                    response.IsSuccess = false;
                }
                else
                {
                    await _roomRepository.DeleteAsync(room); // Використовуємо метод DeleteAsync репозиторію для видалення кімнати
                    response.Data = true;
                    response.Message = "Кімната успішно видалена!";
                    response.IsSuccess = true;
                }
            }
            catch (Exception ex)
            {
                response.Message = $"Помилка при видаленні кімнати: {ex.Message}";
                response.IsSuccess = false;
            }

            return response;
        }

        // Додавання методу GetAll для отримання всіх кімнат
        public async Task<ServiceResponse<List<RoomDto>>> GetAllRoomsAsync()
        {
            var response = new ServiceResponse<List<RoomDto>>();

            try
            {
                // Get all rooms asynchronously from the repository
                var rooms = await _roomRepository.GetAll().ToListAsync();  // Use ToListAsync() to execute the query asynchronously

                response.Data = _mapper.Map<List<RoomDto>>(rooms); // Mapping List<Room> to List<RoomDto>
                response.IsSuccess = true;
                response.Message = "Кімнати успішно отримано!";
            }
            catch (Exception ex)
            {
                response.Message = $"Помилка при отриманні кімнат: {ex.Message}";
                response.IsSuccess = false;
            }

            return response;
        }
    }
}
