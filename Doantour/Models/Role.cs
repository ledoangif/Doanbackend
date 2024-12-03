namespace Doantour.Models
{
    public class Role : BaseEntity
    {

        public string? RoleName { get; set; }
        public ICollection<AppUser>? Users { get; set; } = new List<AppUser>();
    }
}
