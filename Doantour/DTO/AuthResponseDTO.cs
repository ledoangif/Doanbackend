namespace Doantour.DTO
{
    public class AuthResponseDTO
    {
        public string Token { get; set; }
        public string UserId { get; set; }
        public string UserFullName { get; set; }
        public string Email { get; set; }
        public string Role { get; set; }

        //public bool Status { get; set; }

        //public string Message { get; set; }
    }
}
