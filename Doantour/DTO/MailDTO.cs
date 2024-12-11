namespace Doantour.DTO
{
    public class MailDTO
    {
        public string? NameTour { get; set; }
        public int TourId { get; set; }
        public int BookingID { get; set; }
        public DateTime DateEnd { get; set; }
        public DateTime DateStart { get; set; }
        public string? Place { get; set; }
        public string? timeStart { get; set; }
        public string? timeEnd { get; set; }
        public string? placeStart { get; set; }
        public string? placeEnd { get; set; }
        public string? MeetingPoint { get; set; }

        public int NumOfDay { get; set; }
        public decimal PriceSale { get; set; }

        public string CountryName { get; set; }
        public string CustomerName { get; set; }
        public string CustomerPhone { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public decimal TotalBill { get; set; }
        public decimal Paymented { get; set; }
        public string? PaymentBy { get; set; }
        public DateTime PaymentTime { get; set; }
        public int Child { get; set; }
        public int Adult { get; set; }
        public string? customerInTours { get; set; }
        public string Plan { get; set; }
    }
}
