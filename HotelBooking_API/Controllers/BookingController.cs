using HotelBooking.BLL.Services.BookingService;
using HotelBooking.BLL.DTOs.BookingDto;
using Microsoft.AspNetCore.Mvc;

namespace HotelBooking_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookingController : ControllerBase
    {
        private readonly IBookingService _bookingService;

        public BookingController(IBookingService bookingService)
        {
            _bookingService = bookingService;
        }

        // POST: api/Booking/Create
        [HttpPost("Create")]
        public async Task<IActionResult> CreateBooking([FromBody] BookingDto bookingDto)
        {
            if (!ModelState.IsValid)
                return BadRequest("Invalid data.");

            var response = await _bookingService.CreateBookingAsync(bookingDto);

            if (response.IsSuccess)
                return CreatedAtAction(nameof(GetBookingById), new { bookingId = response.Data.Id }, response.Data);

            return BadRequest(response.Message);
        }

        // GET: api/Booking/{bookingId}
        [HttpGet("{bookingId}")]
        public async Task<IActionResult> GetBookingById(int bookingId)
        {
            var response = await _bookingService.GetBookingByIdAsync(bookingId);

            if (!response.IsSuccess)
                return NotFound(response.Message);

            return Ok(response.Data);
        }

        // GET: api/Booking/UserBookings/{userId}
        [HttpGet("UserBookings/{userId}")]
        public async Task<IActionResult> GetUserBookings(string userId)
        {
            var response = await _bookingService.GetUserBookingsAsync(userId);

            if (!response.IsSuccess)
                return NotFound(response.Message);

            return Ok(response.Data);
        }

        // GET: api/Booking/AllBookings
        [HttpGet("AllBookings")]
        public async Task<IActionResult> GetAllBookings()
        {
            var response = await _bookingService.GetAllBookingsAsync();

            if (!response.IsSuccess)
                return NotFound(response.Message);

            return Ok(response.Data);
        }

        // DELETE: api/Booking/Cancel/{bookingId}
        [HttpDelete("Cancel/{bookingId}")]
        public async Task<IActionResult> CancelBooking(int bookingId)
        {
            var response = await _bookingService.CancelBookingAsync(bookingId);

            if (!response.IsSuccess)
                return NotFound(response.Message);

            return Ok(response.Message);
        }
    }
}
