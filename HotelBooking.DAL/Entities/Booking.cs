namespace HotelBooking.DAL.Entities
{
    public class Booking : BaseEntity<int>
    {
        public string UserId { get; set; } = string.Empty;
        public int RoomId { get; set; }
        public DateTime CheckIn { get; set; }
        public DateTime CheckOut { get; set; }
        public string Status { get; set; } = "Pending"; // Pending, Confirmed, Canceled

        public AppUser User { get; set; }
        public Room Room { get; set; }
    }
}
