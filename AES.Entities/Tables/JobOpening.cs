using AES.Shared;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

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

        [ForeignKey("JobID")]
        public virtual Job Job { get; set; }

        [Required]
        public int JobID { get; set; }

        [ForeignKey("StoreID")]
        public virtual Store Store { get; set; }

        [Required]
        public int StoreID { get; set; }

        [Required]
        public OpeningStatus Status { get; set; }

        [StringLength(4000)]
        public string StoreManagerNotes { get; set; }

        [StringLength(4000)]
        public string RequestNotes { get; set; }

        public virtual ICollection<Application> Applications { get; set; }

    }
}
