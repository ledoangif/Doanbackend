namespace Doantour.Request
{
    public class AccountChangePassword
    {
        public string AccountId { get; set; }
        public string OldPassword { get; set; }
        public string NewPassword { get; set; }
    }
}
