using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
    }
}
