namespace Doantour.Request
{
    public class EditAccountRequest
    {
        public string Id { get; set; }
        public string FullName { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public int RoleId { get; set; }
    }
}
