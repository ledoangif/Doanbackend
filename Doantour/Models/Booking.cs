using System.ComponentModel.DataAnnotations.Schema;

namespace Doantour.Models
{
    [Table("Booking")]
    public class Booking : BaseEntity
    {
        public int? TourId { get; set; }
        public int? CustomerId { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal TotalBill { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal Paymented { get; set; }
        //public string? PaymentBy { get; set; }
        public int Child { get; set; }
        public int Adult { get; set; }
        public DateTime PaymentTime { get; set; } = DateTime.Now;
        public string? StatusBill { get; set; }
        public Tour? Tour { get; set; } = null!;
        public Customer? Customer { get; set; } = null!;
        public string? customerInTours { get; set; }


    }
}
