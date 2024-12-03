using Doantour.Models;
using Doantour.Respository;

namespace Doantour.Repository
{
    public class RepositoryFactory
    {
        public RoleRepository RoleRepository { get; set; }
        public TourRepository TourRepository { get; set; }
        public CountryRepository CountryRepository { get; set; }
       
        public HotelRepository HotelRepository { get; set; }
        public BookingRepository BookingRepository { get; set; }
        public CustomerRepository CustomerRepository { get; set; }
        
        public TourHotelRepository TourHotelRepository { get; set; }
        public RepositoryFactory(Hachutravelcontext context, IServiceProvider serviceProvider)
        {
            RoleRepository = new RoleRepository(context);
            TourRepository = new TourRepository(context);
            CountryRepository = new CountryRepository(context);
            HotelRepository = new HotelRepository(context);
            
            CustomerRepository = new CustomerRepository(context);
            BookingRepository = new BookingRepository(context);
            
            TourHotelRepository = new TourHotelRepository(context);

        }
    }
}