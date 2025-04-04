using Microsoft.AspNetCore.Identity;

namespace HotelBooking.DAL.Entities
{
    public class AppUser : IdentityUser, IBaseEntity<string>
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Image { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedDate { get; set; } = DateTime.UtcNow;

        public string? Role { get; set; } // може бути "Admin" або "User"

        public virtual ICollection<AppUserClaim> Claims { get; set; } = new List<AppUserClaim>();
        public virtual ICollection<AppUserLogin> Logins { get; set; } = new List<AppUserLogin>();
        public virtual ICollection<AppUserToken> Tokens { get; set; } = new List<AppUserToken>();
        public virtual ICollection<AppUserRole> UserRoles { get; set; } = new List<AppUserRole>();
        public virtual ICollection<Booking> Bookings { get; set; } = new List<Booking>(); // Додано зв'язок з бронюваннями
    }

    public class AppRole : IdentityRole<string>
    {
        public virtual ICollection<AppUserRole> UserRoles { get; set; } = new List<AppUserRole>();
        public virtual ICollection<AppRoleClaim> RoleClaims { get; set; } = new List<AppRoleClaim>();
    }

    public class AppUserRole : IdentityUserRole<string>
    {
        public virtual AppUser? User { get; set; }
        public virtual AppRole? Role { get; set; }
    }

    public class AppUserClaim : IdentityUserClaim<string>
    {
        public virtual AppUser? User { get; set; }
    }

    public class AppUserLogin : IdentityUserLogin<string>
    {
        public virtual AppUser? User { get; set; }
    }

    public class AppRoleClaim : IdentityRoleClaim<string>
    {
        public virtual AppRole? Role { get; set; }
    }

    public class AppUserToken : IdentityUserToken<string>
    {
        public virtual AppUser? User { get; set; }
    }
}
