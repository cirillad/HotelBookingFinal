using HotelBooking.BLL.DTOs.RoomDto;
using HotelBooking.BLL.Services;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HotelBooking.BLL.Services.RoomService
{
    public interface IRoomService
    {
        Task<ServiceResponse<RoomDto>> AddRoomAsync(RoomDto roomDto);
        Task<ServiceResponse<RoomDto>> GetRoomByIdAsync(int roomId);
        Task<ServiceResponse<List<RoomDto>>> GetAllRoomsAsync();
        Task<ServiceResponse<RoomDto>> UpdateRoomAsync(int roomId, RoomDto roomDto);
        Task<ServiceResponse<bool>> DeleteRoomAsync(int roomId);
    }
}
