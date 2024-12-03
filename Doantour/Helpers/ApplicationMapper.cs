using AutoMapper;
using Doantour.DTO;
using Doantour.Models;
using Doantour.Request;

namespace Doantour.Helpers
{
    public class ApplicationMapper : Profile
    {
        public ApplicationMapper()
        {

            CreateMap<Role, RoleDTO>().ReverseMap();

            CreateMap<Hotel, HotelDTO>().ReverseMap();
            CreateMap<Booking, BookingDTO>().ReverseMap();
            CreateMap<Country, CountryDTO>().ReverseMap();
            CreateMap<Customer, CustomerDTO>().ReverseMap();
            CreateMap<Tour, TourDTO>().ReverseMap();

            CreateMap<TourHotel, TourHotelDTO>().ReverseMap();

            CreateMap<CustomerInTour, CustomerInTourDTO>().ReverseMap();
            CreateMap<AppUser, AccountDTO>().ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id)).ReverseMap();
            CreateMap<EditAccountRequest, AppUser>()
                .ForMember(dest => dest.FullName, opt => opt.MapFrom(src => src.FullName))
                .ForMember(dest => dest.PhoneNumber, opt => opt.MapFrom(src => src.Phone))
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.Email))
                .ForMember(dest => dest.RoleId, opt => opt.MapFrom(src => src.RoleId));

        }

    }
}
