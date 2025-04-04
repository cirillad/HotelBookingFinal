using HotelBooking.DAL.Entities;
using HotelBooking.DAL.Contexts;

namespace HotelBooking.DAL.Repositories.RoomRepos
{
    public class RoomRepository : GenericRepository<Room, int>, IRoomRepository
    {
        public RoomRepository(ApplicationDbContext context) : base(context) { }
    }
}
