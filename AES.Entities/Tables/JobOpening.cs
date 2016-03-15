using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace AES.Entities.Tables
{
    public class JobOpening
    {

        public JobOpening()
        {
            Applications = new HashSet<Application>();
            Stores = new HashSet<Store>();
        }

        [Key]
        [Required]
        public int ID { get; set; }

        [Required]
        public virtual Job Job { get; set; }

        [Required]
        public virtual ICollection<Store> Stores { get; set; }

        public virtual ICollection<Application> Applications { get; set; }

    }
}
