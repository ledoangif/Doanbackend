using System.ComponentModel.DataAnnotations;

namespace Doantour.DTO
{
    public class SendMailResquest
    {
        [Required]
        public string To { get; set; }

        [Required]
        public string Subject { get; set; }

        [Required]
        public string Body { get; set; }
    }
}
