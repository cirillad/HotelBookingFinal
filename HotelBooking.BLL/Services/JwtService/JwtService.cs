using HotelBooking.DAL.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace HotelBooking.BLL.Services.JwtService
{
    public class JwtService
    {
        private readonly IConfiguration _configuration;
        private readonly UserManager<AppUser> _userManager;

        public JwtService(IConfiguration configuration, UserManager<AppUser> userManager)
        {
            _configuration = configuration;
            _userManager = userManager;
        }

        public string GenerateJwtToken(AppUser user)
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
    }
}
