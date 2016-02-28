using System;
using System.ComponentModel.DataAnnotations;

namespace AES.Entities.Tables
{
    class ApplicationMultiAnswer
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
        public bool Answer1 { get; set; }

        [Required]
        public bool Answer2 { get; set; }

        [Required]
        public bool Answer3 { get; set; }

        [Required]
        public bool Answer4 { get; set; }

    }
}
