using Microsoft.AspNetCore.Mvc;
using HotelBooking.BLL.Services.RoomService;
using HotelBooking.BLL.DTOs.RoomDto;
using System.Threading.Tasks;

namespace HotelBooking.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RoomController : ControllerBase
    {
        private readonly IRoomService _roomService;

        // Інжекція залежності через конструктор
        public RoomController(IRoomService roomService)
        {
            _roomService = roomService;
        }

        // Отримання всіх кімнат
        [HttpGet("getall")]
        public async Task<IActionResult> GetAllRooms()
        {
            var result = await _roomService.GetAllRoomsAsync();
            if (result.IsSuccess)
            {
                return Ok(result.Data); // Повертаємо всі кімнати
            }
            return BadRequest(result.Message); // Якщо виникла помилка
        }

        // Отримання кімнати за ID
        [HttpGet("get/{id}")]
        public async Task<IActionResult> GetRoomById(int id)
        {
            var result = await _roomService.GetRoomByIdAsync(id);
            if (result.IsSuccess)
            {
                return Ok(result.Data); // Повертаємо кімнату за ID
            }
            return NotFound(result.Message); // Якщо кімната не знайдена
        }

        // Додавання нової кімнати
        [HttpPost("add")]
        public async Task<IActionResult> AddRoom([FromBody] RoomDto roomDto)
        {
            if (roomDto == null)
            {
                return BadRequest("Невірні дані для кімнати.");
            }

            var result = await _roomService.AddRoomAsync(roomDto);
            if (result.IsSuccess)
            {
                return CreatedAtAction(nameof(GetRoomById), new { id = result.Data.Id }, result.Data); // Створено
            }
            return BadRequest(result.Message); // Якщо виникла помилка при додаванні
        }

        // Оновлення кімнати
        [HttpPut("update/{id}")]
        public async Task<IActionResult> UpdateRoom(int id, [FromBody] RoomDto roomDto)
        {
            if (roomDto == null || id != roomDto.Id)
            {
                return BadRequest("Некоректні дані або ID не співпадають.");
            }

            var result = await _roomService.UpdateRoomAsync(id, roomDto);
            if (result.IsSuccess)
            {
                return Ok(result.Data); // Повертаємо оновлену кімнату
            }
            return NotFound(result.Message); // Якщо кімната не знайдена
        }

        // Видалення кімнати
        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> DeleteRoom(int id)
        {
            var result = await _roomService.DeleteRoomAsync(id);
            if (result.IsSuccess)
            {
                return NoContent(); // Кімната успішно видалена
            }
            return NotFound(result.Message); // Якщо кімната не знайдена
        }
    }
}
