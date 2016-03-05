using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace AES.Entities.Tables
{
    public class JobOpening
    {
        [Key]
        [Required]
        public int ID { get; set; }

        [Required]
        public virtual Job Job { get; set; }

        [Required]
        public virtual Store Store { get; set; }

        public virtual ICollection<Application> Applications { get; set; }

    }
}
