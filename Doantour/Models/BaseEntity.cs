using System.ComponentModel.DataAnnotations;

namespace Doantour.Models
{
    public abstract class BaseEntity
    {
        [Key]
        public int Id { get; set; }
        
        public DateTime UpdateDate { get; set; }
        public Boolean IsDeleted { get; set; } = false;
    }
}
