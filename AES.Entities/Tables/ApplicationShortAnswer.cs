using System;
using System.ComponentModel.DataAnnotations;

namespace AES.Entities.Tables
{
    public class ApplicationShortAnswer
    {
        [Key]
        [Required]
        public int ID { get; set; }

        /// <summary>
        /// Which question this is answering
        /// </summary>
        [Required]
        public virtual JobQuestion Question { get; set; }

        [Required]
        [StringLength(4000)]
        public string Answer { get; set; }

    }
}
