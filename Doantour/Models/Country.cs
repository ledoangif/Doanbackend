using System.ComponentModel.DataAnnotations.Schema;

namespace Doantour.Models
{
    [Table("Country")]
    public class Country : BaseEntity
    {
        public string? CountryName { get; set; }
        public string? ContinentName { get; set; }
        public string? CountryImage { get; set; }
        public ICollection<Tour>? Tours { get; set; } = new List<Tour>();
    }
}
