namespace Doantour.DTO
{
    public class TourDTO : BaseDTO
    {
        public string? NameTour { get; set; }
        public string? Descripttion { get; set; }
        public int Seat { get; set; }
        public decimal Cost { get; set; }
        public decimal PriceBuy { get; set; }
        public int slot { get; set; }
        public int? Discount { get; set; }
        public bool IsLocal { get; set; }
        public DateTime? DateEnd { get; set; }
        public DateTime? DateStart { get; set; }
        public string? Place { get; set; }
        public string? timeStart { get; set; }
        public string? timeEnd { get; set; }
        public string? placeStart { get; set; }
        public string? placeEnd { get; set; }
        public string? Plan { get; set; }
        public string? IsFinish { get; set; }
        public string? Image { get; set; }
        public int NumOfDay { get; set; }
        public decimal PriceSale { get; set; }
        public string? ServiceInclude { get; set; }
        public string? ServiceNotInclude { get; set; }
        public string? MeetingPoint { get; set; }
        public int? CountryId { get; set; }
        public string? AppUserId { get; set; }
        
    }
}