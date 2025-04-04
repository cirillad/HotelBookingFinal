using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelBooking.BLL.DTOs.RoomDto
{
    public class RoomDto
    {
        public int Id { get; set; }         // Ідентифікатор кімнати
        public string RoomNumber { get; set; } // Номер кімнати
        public string Type { get; set; }      // Тип кімнати
        public decimal Price { get; set; }    // Ціна кімнати
        public bool IsAvailable { get; set; } // Статус доступності кімнати
    }
}
