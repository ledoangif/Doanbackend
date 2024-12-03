namespace Doantour.DTO
{
    public class BaseDTO
    {
        public int Id { get; set; }
        
        public DateTime UpdateDate { get; set; }
        public Boolean IsDeleted { get; set; } = false;
    }
}

