using System.ComponentModel.DataAnnotations.Schema;

namespace Doantour.Models
{
    [Table("Tour")]
    public class Tour : BaseEntity
    {
        public string? NameTour { get; set; }
        public string? Descripttion { get; set; }
        public int Seat { get; set; }
        public int slot { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal? PriceBuy { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal Cost { get; set; }
        public int? Discount { get; set; }
        public bool IsLocal { get; set; }
        public string? Image { get; set; }
        public DateTime? DateStart { get; set; }
        public DateTime? DateEnd { get; set; }
        public int NumOfDay { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal PriceSale { get; set; }
        public string? ServiceInclude { get; set; }
        public string? ServiceNotInclude { get; set; }

        public string? Plan { get; set; }
        public string? Place { get; set; }
        public string? timeStart { get; set; }
        public string? timeEnd { get; set; }
        public string? placeStart { get; set; }
        public string? placeEnd { get; set; }


        public string? IsFinish { get; set; }
        public string? MeetingPoint { get; set; }
        public int? CountryId { get; set; }
        public string? AppUserId { get; set; }
        public Country? Country { get; set; } = null!;
        public ICollection<TourHotel>? TourHotels { get; set; } = new List<TourHotel>();
        
        public AppUser? AppUser { get; set; } = null!;
        public ICollection<Booking>? Bookings { get; set; } = new List<Booking>();

    }
}
