using Doantour.DTO;
using Doantour.Models;

namespace Doantour.Service
{
    public interface IBookingService : IBaseService<Booking, BookingDTO>
    {
        Task<List<BookingDTO>> GetBookingsByDate(int day, int month, int year);
    }
}
