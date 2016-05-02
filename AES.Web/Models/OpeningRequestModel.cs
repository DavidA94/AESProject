using AES.Shared;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AES.Web.Models
{
    public class OpeningRequestModel
    {
        [Required]
        public int RequestID { get; set; }

        [Required]
        public string JobTitle { get; set; }

        [Required]
        public string JobShortDescription { get; set; }

        public string StoreManagerNotes { get; set; }

        public string RequestNotes { get; set; }

        [Required]
        public int NumOpenings { get; set; }

        [Required]
        public OpeningStatus Status { get; set; }
    }
}
