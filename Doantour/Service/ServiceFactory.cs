using AutoMapper;
using Doantour.Models;
using Microsoft.AspNetCore.Identity;

namespace Doantour.Service
{
    public class ServiceFactory
    {
        public RoleService RoleService { get; set; }
        public TourService TourService { get; set; }
      
        public AccountService AccountService { get; set; }
        public CountryService CountryService { get; set; }
        public HotelService HotelService { get; set; }
        public BookingService BookingService { get; set; }
        public CustomerService CustomerService { get; set; }
  
        public TourHotelService TourHotelService { get; set; }
        public SendMailService SendMailService { get; set; }

        public ServiceFactory(IMapper mapper, IServiceProvider serviceProvider,
            UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, IConfiguration configuration
            )
        {
            RoleService = new RoleService(mapper, serviceProvider);
            AccountService = new AccountService(mapper, serviceProvider, userManager, signInManager, configuration);
            
            TourService = new TourService(mapper, serviceProvider);
            CountryService = new CountryService(mapper, serviceProvider);
            HotelService = new HotelService(mapper, serviceProvider);
            BookingService = new BookingService(mapper, serviceProvider);
            CustomerService = new CustomerService(mapper, serviceProvider);
           
            TourHotelService = new TourHotelService(mapper, serviceProvider);
            SendMailService = new SendMailService(mapper, serviceProvider);
        }

    }
}
