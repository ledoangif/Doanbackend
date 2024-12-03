using Doantour.DTO;
using Doantour.Helpers;
using Doantour.Models;
using Doantour.Repository;
using Microsoft.EntityFrameworkCore;

namespace Doantour.Respository
{
    public class TourRepository : BaseRepository<Tour>
    {
        private readonly Hachutravelcontext _context;
        public TourRepository(Hachutravelcontext context) : base(context)
        {
            _context = context;
        }

        public Task<List<CustomerByTour>> GetCustomerByTour(int id)
        {
            var items = from a in _context.Tour
                        join b in _context.Booking on a.Id equals b.TourId
                        join c in _context.Customer on b.CustomerId equals c.Id
                        where b.StatusBill == Constants.Success && b.IsDeleted == false && a.Id == id
                        select new CustomerByTour()
                        {
                            NameCustomer = c.NameCustomer,
                            phoneNumber = c.PhoneNumber,
                            BookingId = b.Id,
                            customerinTour = b.customerInTours,
                        };
            return items.ToListAsync();
        }

    }
}
