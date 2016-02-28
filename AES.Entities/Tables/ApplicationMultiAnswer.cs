using System;
using System.ComponentModel.DataAnnotations;

namespace AES.Entities.Tables
{
    class ApplicationMultiAnswer
    {
        [Key]
        [Required]
        public int ID { get; set; }

        /// <summary>
        /// Which question this is answering
        /// </summary>
        [Required]
        public JobQuestion Question { get; set; }

        /// <summary>
        /// Which application this answer is in
        /// </summary>
        [Required]
        public Application Application { get; set; }

        [Required]
        public bool Answer1 { get; set; }

        [Required]
        public bool Answer2 { get; set; }

        public bool Answer3 { get; set; }

        public bool Answer4 { get; set; }

    }
}
