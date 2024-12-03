namespace Doantour.DTO
{
    public class CustomerDTO : BaseDTO
    {
        public string? NameCustomer { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Address { get; set; }
        public string? Email { get; set; }
        public string? IdentityCard { get; set; }
        public string? AccountBank { get; set; }
        public string? BankName { get; set; }
        public bool Gender { get; set; }
    }
}
