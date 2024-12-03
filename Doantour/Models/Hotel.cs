using System.ComponentModel.DataAnnotations.Schema;

namespace Doantour.Models
{
    [Table("Hotel")]
    public class Hotel : BaseEntity
    {
        public string? HotelName { get; set; }
        public int Rate { get; set; }
        public string? Address { get; set; }
        public string? Hotline { get; set; }
        public string? CEO { get; set; }
        public string? LinkHotel { get; set; }
        public ICollection<TourHotel>? TourHotels { get; set; } = new List<TourHotel>();

    }
}
