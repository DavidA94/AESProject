using System;
using System.ComponentModel.DataAnnotations;

namespace AES.Entities.Tables
{
    class ApplicationShortAnswer
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
        [StringLength(4000)]
        public string Answer { get; set; }

    }
}
