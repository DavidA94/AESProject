using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
