using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using HotelBooking.DAL.Entities;

namespace HotelBooking.DAL.Contexts
{
    public class ApplicationDbContext : IdentityDbContext<AppUser, AppRole, string,
        AppUserClaim, AppUserRole, AppUserLogin, AppRoleClaim, AppUserToken>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<Room> Rooms { get; set; }
        public DbSet<Booking> Bookings { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // Ensure AppRole ID is generated automatically
            builder.Entity<AppRole>()
                .Property(r => r.Id)
                .ValueGeneratedOnAdd();

            // Configure AppUser and AppRole relationships
            builder.Entity<AppUser>(b =>
            {
                b.HasMany(u => u.UserRoles)
                 .WithOne(ur => ur.User)
                 .HasForeignKey(ur => ur.UserId)
                 .IsRequired();
            });

            builder.Entity<AppRole>(b =>
            {
                b.HasMany(r => r.UserRoles)
                 .WithOne(ur => ur.Role)
                 .HasForeignKey(ur => ur.RoleId)
                 .IsRequired();
            });

            builder.Entity<Booking>()
                .HasOne(b => b.User)
                .WithMany(u => u.Bookings)
                .HasForeignKey(b => b.UserId);
        }
    }
}
