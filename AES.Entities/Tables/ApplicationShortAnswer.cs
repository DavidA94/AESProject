using System;
using System.ComponentModel.DataAnnotations;

namespace AES.Entities.Tables
{
    class ApplicationShortAnswer
    {
        [Key]
        [Required]
        public int ID { get; set; }

        // Which question this is answering
        [Required]
        public JobQuestion Question { get; set; }

        // Which application this answer is in
        [Required]
        public Application Application { get; set; }

        [Required]
        [StringLength(4000)]
        public string Answer { get; set; }

    }
}
