using System.ComponentModel.DataAnnotations.Schema;

namespace Doantour.Models
{

    [Table("TourHotel")]
    public class TourHotel : BaseEntity
    {
        public int TourId { get; set; }
        public Tour Tour { get; set; } = null!;

        public int HotelId { get; set; }
        public Hotel Hotel { get; set; } = null!;

        public DateTime dateStart { get; set; }
        public DateTime dateEnd { get; set; }
        public string? Note { get; set; }
    }
}
