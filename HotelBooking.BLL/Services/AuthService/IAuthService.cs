using HotelBooking.BLL.DTOs.Account;
using HotelBooking.BLL.DTOs.UserDto;
using HotelBooking.DAL.Entities;


namespace HotelBooking.BLL.Services.AuthService
{
    public interface IAuthService
    {
        Task<ServiceResponse<string?>> LoginAsync(LoginDto dto);
        Task<ServiceResponse<string?>> RegisterAsync(RegisterDto dto);
        Task<ServiceResponse<bool>> ConfirmEmailAsync(string id, string token);
        Task<ServiceResponse<bool>> SendConfirmEmailTokenAsync(string userId);
        Task<ServiceResponse<List<AppUser>>> GetUsersByRoleAsync(string role);
        Task<ServiceResponse<List<AppUser>>> GetSortedUsersAsync(string sortBy);
        Task<ServiceResponse<List<AppUser>>> GetAllUsersAsync();
        Task<ServiceResponse<AppUser?>> UpdateUserAsync(Guid userId, UpdateUserDto dto);
    }
}
