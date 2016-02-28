using System;
using System.ComponentModel.DataAnnotations;

namespace AES.Entities.Tables
{
    class JobQuestion
    {

        public enum QuestionType { SHORT, RADIO, CHECKBOX }

        [Key]
        [Required]
        public int ID { get; set; }

        /// <summary>
        /// Which job this question is for
        /// </summary>
        [Required]
        public Job Job { get; set; }

        [Required]
        public QuestionType Type { get; set; }

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

        /// <summary>
        /// A list, in any order, of the numbers corresponding to correct answers (1 through 4)
        /// Can be things like "12", "14", "1", 3", "123", "21"
        /// </summary>
        [Required]
        [StringLength(4)]
        public string CorrectAnswers { get; set; }

        /// <summary>
        /// How many answers must be correct
        /// </summary>
        [Required]
        [Range(0, 4)]
        public int CorrectAnswerThreshold { get; set; }

    }
}
