using AES.Shared;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace AES.Entities.Tables
{
    public class JobOpening
    {

        public JobOpening()
        {
            Applications = new HashSet<Application>();
        }

        [Key]
        [Required]
        public int ID { get; set; }

        [Required]
        public virtual Job Job { get; set; }

        [Required]
        public virtual Store Store { get; set; }

        [Required]
        public OpeningStatus Status { get; set; }

        [StringLength(4000)]
        public string StoreManagerNotes { get; set; }

        public virtual ICollection<Application> Applications { get; set; }

    }
}
