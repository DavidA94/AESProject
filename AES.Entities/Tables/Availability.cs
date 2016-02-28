using System.ComponentModel.DataAnnotations;

namespace AES.Entities.Tables
{
    public class Availability
    {
        [Key]
        [Required]
        public int ID { get; set; }

        [Required]
        [StringLength(128)]
        public string Sunday { get; set; }

        [Required]
        [StringLength(128)]
        public string Monday { get; set; }

        [Required]
        [StringLength(128)]
        public string Tuesday { get; set; }

        [Required]
        [StringLength(128)]
        public string Wednesday { get; set; }

        [Required]
        [StringLength(128)]
        public string Thursday { get; set; }

        [Required]
        [StringLength(128)]
        public string Friday { get; set; }

        [Required]
        [StringLength(128)]
        public string Saturday { get; set; }


    }
}
