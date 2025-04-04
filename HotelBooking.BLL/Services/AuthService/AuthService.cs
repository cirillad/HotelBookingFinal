using AutoMapper;
using HotelBooking.BLL.DTOs.Account;
using HotelBooking.BLL.Services.JwtService;
using HotelBooking.BLL.Services.EmailService;
using HotelBooking.DAL.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using HotelBooking.BLL.DTOs.UserDto;

namespace HotelBooking.BLL.Services.AuthService
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<AppRole> _roleManager;
        private readonly IConfiguration _configuration;
        private readonly IMapper _mapper;
        private readonly IEmailService _emailService;  // Додано для використання EmailService

        public AuthService(UserManager<AppUser> userManager, RoleManager<AppRole> roleManager, IConfiguration configuration, IMapper mapper, IEmailService emailService)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _configuration = configuration;
            _mapper = mapper;
            _emailService = emailService;  // Ін'єкція EmailService
        }

        public async Task<ServiceResponse<string>> LoginAsync(LoginDto dto)
        {
            var user = await _userManager.FindByNameAsync(dto.UserName ?? "");

            if (user == null)
                return new ServiceResponse<string>(null, $"Користувача з іменем '{dto.UserName}' не знайдено", false);

            var isPasswordValid = await _userManager.CheckPasswordAsync(user, dto.Password ?? "");
            if (!isPasswordValid)
                return new ServiceResponse<string>(null, "Пароль вказано невірно", false);

            var jwtToken = GenerateJwtToken(user);  // Генеруємо JWT токен

            return new ServiceResponse<string>(jwtToken, "Успішний вхід", true);
        }



        public async Task<ServiceResponse<string>> RegisterAsync(RegisterDto dto)
        {
            // Check if email is already taken
            if (await _userManager.FindByEmailAsync(dto.Email) != null)
                return new ServiceResponse<string>(null, $"Email '{dto.Email}' зайнятий", false);

            // Check if username is already taken
            if (await _userManager.FindByNameAsync(dto.UserName) != null)
                return new ServiceResponse<string>(null, $"Ім'я '{dto.UserName}' вже використовується", false);

            // Map DTO to AppUser
            var user = _mapper.Map<AppUser>(dto);

            // Create the user
            var result = await _userManager.CreateAsync(user, dto.Password);

            // Check if the user creation was successful
            if (!result.Succeeded)
                return new ServiceResponse<string>(null, string.Join("; ", result.Errors.Select(e => e.Description)), false);

            // Assign the "user" role if it exists
            if (await _roleManager.RoleExistsAsync("User"))
                await _userManager.AddToRoleAsync(user, "User");

            // Generate JWT token
            var jwtToken = GenerateJwtToken(user);  // Assuming this is a method that generates the token

            // Return the token in the response
            return new ServiceResponse<string>(jwtToken, "Успішна реєстрація", true);
        }



        public async Task<ServiceResponse<bool>> ConfirmEmailAsync(string userId, string token)
        {
            // Знаходимо користувача за ID
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
                return new ServiceResponse<bool>(false, "Користувача не знайдено", false);

            // Декодуємо токен, так як він передається в закодованому вигляді (Base64)
            var decodedToken = Encoding.UTF8.GetString(Convert.FromBase64String(token));

            // Підтверджуємо email за допомогою декодованого токена
            var result = await _userManager.ConfirmEmailAsync(user, decodedToken);

            // Повертаємо результат
            return new ServiceResponse<bool>(result.Succeeded,
                result.Succeeded ? "Email підтверджено успішно" : "Не вдалося підтвердити email",
                result.Succeeded);
        }


        public async Task<ServiceResponse<bool>> SendConfirmEmailTokenAsync(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
                return new ServiceResponse<bool>(false, "Користувача не знайдено", false);

            // Генерація токена для підтвердження електронної пошти
            var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            var encodedToken = Convert.ToBase64String(Encoding.UTF8.GetBytes(token));

            // Формуємо URL для підтвердження
            var confirmEmailUrl = $"{_configuration["AppSettings:FrontendUrl"]}/confirm-email?token={encodedToken}&userId={userId}";

            // Формуємо контент листа
            var emailSubject = "Підтвердження електронної пошти";
            var emailBody = $"Вітаємо, {user.UserName}!<br><br>Щоб підтвердити свою електронну пошту, натисніть на посилання нижче:<br><a href='{confirmEmailUrl}'>Підтвердити електронну пошту</a>";

            // Надсилаємо лист
            await _emailService.SendMailAsync(user.Email!, emailSubject, emailBody, true);

            return new ServiceResponse<bool>(true, "Лист із підтвердженням надіслано", true);
        }


        public async Task<ServiceResponse<List<AppUser>>> GetUsersByRoleAsync(string role)
        {
            var roleExists = await _roleManager.RoleExistsAsync(role);
            if (!roleExists)
                return new ServiceResponse<List<AppUser>>(new List<AppUser>(), $"Роль '{role}' не знайдено", false);

            var users = await _userManager.GetUsersInRoleAsync(role);
            return new ServiceResponse<List<AppUser>>(users.ToList(), "Користувачі знайдені", true);
        }

        public async Task<ServiceResponse<List<AppUser>>> GetSortedUsersAsync(string sortBy)
        {
            var users = _userManager.Users.AsQueryable();

            users = sortBy.ToLower() switch
            {
                "email" => users.OrderBy(u => u.Email),
                "username" => users.OrderBy(u => u.UserName),
                _ => users.OrderBy(u => u.Id)
            };

            return new ServiceResponse<List<AppUser>>(users.ToList(), "Користувачі відсортовані", true);
        }

        private string GenerateJwtToken(AppUser user)
        {
            var claims = new List<Claim>
            {
                new Claim("id", user.Id),
                new Claim("email", user.Email ?? ""),
                new Claim("userName", user.UserName ?? "")
            };

            var roles = _userManager.GetRolesAsync(user).Result;
            if (roles.Any())
                claims.AddRange(roles.Select(r => new Claim(ClaimTypes.Role, r)));

            var key = Encoding.UTF8.GetBytes(_configuration["JwtSettings:Key"] ?? throw new ArgumentNullException("JwtSettings:Key"));
            var token = new JwtSecurityToken(
                issuer: _configuration["JwtSettings:Issuer"],
                audience: _configuration["JwtSettings:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(int.Parse(_configuration["JwtSettings:ExpTime"] ?? "60")),
                signingCredentials: new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256)
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public async Task<ServiceResponse<List<AppUser>>> GetAllUsersAsync()
        {
            var users = await _userManager.Users.ToListAsync();
            return new ServiceResponse<List<AppUser>>(users, "Усі користувачі отримані успішно", true);
        }

        public async Task<ServiceResponse<AppUser?>> UpdateUserAsync(Guid userId, UpdateUserDto dto)
        {
            // Знаходимо користувача за GUID
            var user = await _userManager.FindByIdAsync(userId.ToString());
            if (user == null)
                return new ServiceResponse<AppUser?>(null, "Користувача не знайдено", false);

            // Оновлення властивостей, якщо передано
            if (!string.IsNullOrWhiteSpace(dto.UserName))
                user.UserName = dto.UserName;

            if (!string.IsNullOrWhiteSpace(dto.Email))
                user.Email = dto.Email;

            if (!string.IsNullOrWhiteSpace(dto.FirstName))
                user.FirstName = dto.FirstName;

            if (!string.IsNullOrWhiteSpace(dto.LastName))
                user.LastName = dto.LastName;

            user.UpdatedDate = DateTime.UtcNow;

            var result = await _userManager.UpdateAsync(user);
            if (!result.Succeeded)
                return new ServiceResponse<AppUser?>(null, string.Join("; ", result.Errors.Select(e => e.Description)), false);

            // Якщо потрібно оновити роль
            if (!string.IsNullOrWhiteSpace(dto.Role))
            {
                var currentRoles = await _userManager.GetRolesAsync(user);
                await _userManager.RemoveFromRolesAsync(user, currentRoles);
                if (await _roleManager.RoleExistsAsync(dto.Role))
                    await _userManager.AddToRoleAsync(user, dto.Role);
            }

            return new ServiceResponse<AppUser?>(user, "Користувача оновлено успішно", true);
        }


    }
}
