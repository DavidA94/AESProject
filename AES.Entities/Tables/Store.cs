using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace AES.Entities.Tables
{
    public class Store
    {

        public Store()
        {
            Openings = new HashSet<JobOpening>();
        }

        [Key]
        [Required]
        public int ID { get; set; }

        [Required]
        public bool IsActive { get; set; }

        [StringLength(50)]
        [Required]
        public string Name { get; set; }

        [StringLength(50)]
        [Required]
        public string Address { get; set; }

        [StringLength(30)]
        [Required]
        public string City { get; set; }

        [StringLength(2)]
        [Required]
        public string State { get; set; }

        [Range(0, 99999)]
        [Required]
        public int Zip { get; set; }

        [RegularExpression(@"\d{3}-\d{3}-\d{4}")]
        [Required]
        public string Phone { get; set; }

        public virtual ICollection<JobOpening> Openings { get; set; }

    }
}
