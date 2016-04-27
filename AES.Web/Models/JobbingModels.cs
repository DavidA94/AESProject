using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AES.Web.Models
{
    public class JobModel
    {
        public JobModel()
        {
            JobID = -1;
        }

        /// <summary>
        /// The ID of this job
        /// </summary>
        [Required]
        public int JobID { get; set; }

        /// <summary>
        /// The title of this job
        /// </summary>
        [Required]
        [StringLength(128)]
        [Display(Name = "Job Title")]
        public string Title { get; set; }

        /// <summary>
        /// The short description for this job
        /// </summary>
        [Required]
        [StringLength(512)]
        [Display(Name = "Short Description")]
        public string ShortDescription { get; set; }

        /// <summary>
        /// The long description for this job
        /// </summary>
        [Required]
        [StringLength(4000)]
        [Display(Name = "Long Description")]
        public string LongDescription { get; set; }
    }
}
