using System.ComponentModel.DataAnnotations.Schema;

namespace Doantour.DTO
{
    public class BookingDTO : BaseDTO
    {
        public int? TourId { get; set; }
        public int? CustomerId { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal TotalBill { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal Paymented { get; set; }
        //public string? PaymentBy { get; set; }
        public DateTime PaymentTime { get; set; }
        public int Child { get; set; }
        public int Adult { get; set; }
        public string? StatusBill { get; set; }
        public string? customerInTours { get; set; }
    }
}
