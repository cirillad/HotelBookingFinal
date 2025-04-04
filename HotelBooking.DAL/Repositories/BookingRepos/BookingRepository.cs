using HotelBooking.DAL.Contexts;
using HotelBooking.DAL.Entities;

namespace HotelBooking.DAL.Repositories.BookingRepos
{
    public class BookingRepository : GenericRepository<Booking, int>, IBookingRepository
    {
        public BookingRepository(ApplicationDbContext context) : base(context) { }
    }
}
