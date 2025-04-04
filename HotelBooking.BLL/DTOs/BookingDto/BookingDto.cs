namespace HotelBooking.BLL.DTOs.BookingDto
{
    public class BookingDto
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public int RoomId { get; set; }
        public DateTime CheckIn { get; set; }
        public DateTime CheckOut { get; set; }
        public string Status { get; set; }

        // Можливо, додамо властивості для включення даних користувача та кімнати
        public string UserName { get; set; }  // Ім'я користувача
        public string RoomName { get; set; }  // Назва кімнати
    }
}
