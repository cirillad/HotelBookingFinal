using HotelBooking.DAL.Contexts;
using HotelBooking.DAL.Entities;

namespace HotelBooking.DAL.Repositories.Users
{
    public class UserRepository : GenericRepository<AppUser, string>, IUserRepository
    {
        public UserRepository(ApplicationDbContext context)
            : base(context) { }
    }
}
