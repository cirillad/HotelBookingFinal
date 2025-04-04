using HotelBooking.BLL.DTOs.Account;
using HotelBooking.BLL.DTOs.UserDto;
using HotelBooking.BLL.Services.AuthService;
using HotelBooking.DAL.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace HotelBooking_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly UserManager<AppUser> _userManager;

        public AuthController(IAuthService authService, UserManager<AppUser> userManager)
        {
            _authService = authService;
            _userManager = userManager;
        }

        // POST: api/Auth/Register
        [HttpPost("Register")]
        public async Task<IActionResult> Register([FromBody] RegisterDto model)
        {
            if (!ModelState.IsValid)
                return BadRequest("Invalid registration data.");

            var response = await _authService.RegisterAsync(model);
            if (!response.IsSuccess)
                return BadRequest(response.Message);

            return Ok(new { JwtToken = response.Data });
        }


        // POST: api/Auth/Login
        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromBody] LoginDto model)
        {
            if (!ModelState.IsValid)
                return BadRequest("Invalid login data.");

            var response = await _authService.LoginAsync(model);
            if (!response.IsSuccess)
                return Unauthorized(response.Message);

            return Ok(new { JwtToken = response.Data });
        }



        // GET: api/Auth/ConfirmEmail
        [HttpGet("ConfirmEmail")]
        public async Task<IActionResult> ConfirmEmail([FromQuery] string userId, [FromQuery] string token)
        {
            if (string.IsNullOrEmpty(userId) || string.IsNullOrEmpty(token))
                return BadRequest("UserId і Token обов'язкові для підтвердження email.");

            var response = await _authService.ConfirmEmailAsync(userId, token);
            if (!response.IsSuccess)
                return BadRequest(response.Message);

            return Ok(response);
        }


        // POST: api/Auth/SendConfirmEmailToken
        [HttpPost("SendConfirmEmailToken")]
        public async Task<IActionResult> SendConfirmEmailToken([FromQuery] string userId)
        {
            var response = await _authService.SendConfirmEmailTokenAsync(userId);
            if (!response.IsSuccess)
                return BadRequest(response.Message);

            return Ok(response);
        }

        // GET: api/Auth/GetUsersByRole
        [HttpGet("GetUsersByRole")]
        public async Task<IActionResult> GetUsersByRole([FromQuery] string role)
        {
            var response = await _authService.GetUsersByRoleAsync(role);
            if (!response.IsSuccess)
                return BadRequest(response.Message);

            return Ok(response);
        }

        // GET: api/Auth/GetSortedUsers
        [HttpGet("GetSortedUsers")]
        public async Task<IActionResult> GetSortedUsers([FromQuery] string sortBy)
        {
            var response = await _authService.GetSortedUsersAsync(sortBy);
            if (!response.IsSuccess)
                return BadRequest(response.Message);

            return Ok(response);
        }

        // GET: api/Auth/Users
        [HttpGet("AllUsers")]
        public async Task<IActionResult> GetAllUsers()
        {
            var response = await _authService.GetAllUsersAsync();
            if (!response.IsSuccess)
                return BadRequest(response.Message);

            return Ok(response.Data);
        }

        [HttpGet("test-error")]
        public IActionResult TestError()
        {
            throw new Exception("Тестова помилка сервера");
        }

        // PUT: api/Auth/UpdateUser
        [HttpPut("UpdateUser")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> UpdateUser([FromBody] UpdateUserDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest("Невірні дані для оновлення.");

            // Ensure the userId is passed in the request body
            if (dto.UserId == Guid.Empty)
                return BadRequest("Invalid user ID.");

            // Now pass userId and dto to the service
            var response = await _authService.UpdateUserAsync(dto.UserId, dto);
            if (!response.IsSuccess)
                return BadRequest(response.Message);

            return Ok(response);
        }





    }
}
