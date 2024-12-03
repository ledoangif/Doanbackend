using System.ComponentModel.DataAnnotations.Schema;

namespace Doantour.Models
{
    [Table("Customer")]
    public class Customer : BaseEntity
    {
        public string? NameCustomer { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Address { get; set; }
        public string? Email { get; set; }
        public string? IdentityCard { get; set; }
        public string? AccountBank { get; set; }
        public string? BankName { get; set; }
        public bool Gender { get; set; }
        public ICollection<Booking>? Bookings { get; set; } = new List<Booking>();
    }
}
