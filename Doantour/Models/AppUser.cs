using Microsoft.AspNetCore.Identity;

namespace Doantour.Models
{

    public class AppUser : IdentityUser
    {

        public string? FullName { get; set; }
        public string? Phone { get; set; }
        public Role? Role { get; set; } = null!;
        public int? RoleId { get; set; }
        public ICollection<Tour>? Tours { get; set; } = new List<Tour>();


    }
}
