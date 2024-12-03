namespace Doantour.DTO
{
    public class TourHotelDTO : BaseDTO
    {
        public int TourId { get; set; }

        public int HotelId { get; set; }

        public DateTime dateStart { get; set; }
        public DateTime dateEnd { get; set; }
        public string? Note { get; set; }
    }
}
