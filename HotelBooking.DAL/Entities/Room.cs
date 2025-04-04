namespace HotelBooking.DAL.Entities
{
    public class Room : BaseEntity<int>
    {
        public int Number { get; set; }
        public string Type { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public bool IsAvailable { get; set; } = true;

        public ICollection<Booking> Bookings { get; set; } = new List<Booking>();
    }
}