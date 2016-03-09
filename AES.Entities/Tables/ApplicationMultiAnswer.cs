﻿using System;
using System.ComponentModel.DataAnnotations;

namespace AES.Entities.Tables
{
    public class ApplicationMultiAnswer
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
        public bool Answer1 { get; set; }

        [Required]
        public bool Answer2 { get; set; }

        [Required]
        public bool Answer3 { get; set; }

        [Required]
        public bool Answer4 { get; set; }

    }
}
