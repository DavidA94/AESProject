using System.ComponentModel.DataAnnotations;

namespace AES.Entities.Tables
{
    public class Job
    {
        [Key]
        [Required]
        public int ID { get; set; }

        [StringLength(512)]
        [Required]
        public string descShort { get; set; }

        [StringLength(4000)]
        [Required]
        public string descLong { get; set; }

    }
}
