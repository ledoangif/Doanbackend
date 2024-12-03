using Doantour.DTO;
using Microsoft.AspNetCore.SignalR;

namespace Doantour.Hubs
{
    public class BookingHub : Hub
    {
        public async Task NotifyNewBooking(BookingDTO bookingDTO)
        {
            await Clients.All.SendAsync("ReceiveNewBookingNotification", bookingDTO);
        }
    }
}
