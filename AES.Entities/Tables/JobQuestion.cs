using System;
using System.ComponentModel.DataAnnotations;

namespace AES.Entities.Tables
{
    class JobQuestion
    {

        [Key]
        [Required]
        public int ID { get; set; }

        // Which job this question is for
        [Required]
        public Job Job { get; set; }

        // 1 for short answer, 2 for radio multi choice, 3 for checkbox multi choice
        [Required]
        [Range(1, 3)]
        public int Type { get; set; }

        [Required]
        [StringLength(4000)]
        public string Text { get; set; }

        [StringLength(128)]
        public string Option1 { get; set; }

        [StringLength(128)]
        public string Option2 { get; set; }

        [StringLength(128)]
        public string Option3 { get; set; }

        [StringLength(128)]
        public string Option4 { get; set; }

        // Can be things like "12", "14", "1", 3", "123", "21"
        [Required]
        [StringLength(4)]
        public string CorrectAnswers { get; set; }

        // How many answers must be correct
        [Required]
        [Range(0, 4)]
        public int CorrectAnswerThreshold { get; set; }

    }
}
