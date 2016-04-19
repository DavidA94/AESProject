using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AES.Entities.Tables
{
    public class Job
    {

        public Job()
        {
            Openings = new HashSet<JobOpening>();
            Questions = new HashSet<JobQuestion>();
        }

        [Key]
        [Required]
        public int JobID { get; set; }

        [StringLength(128)]
        [Index(IsUnique = true)]
        public string Title { get; set; }

        [StringLength(512)]
        [Index(IsUnique = true)]
        public string ShortDescription { get; set; }

        [StringLength(4000)]
        [Required]
        public string LongDescription { get; set; }

        public virtual ICollection<JobOpening> Openings { get; set; }

        public virtual ICollection<JobQuestion> Questions { get; set; }

    }
}
