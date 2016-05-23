using System.ComponentModel.DataAnnotations;

namespace AES.Web.Models
{
    public class StoreModel
    {
        [Required]
        public int StoreID { get; set; }

        [StringLength(50)]
        [Required]
        [Display(Name = "Store Name")]
        public string Name { get; set; }

        [StringLength(50)]
        [Required]
        public string Address { get; set; }

        [StringLength(30)]
        [Required]
        public string City { get; set; }

        [StringLength(2)]
        [Required]
        public string State { get; set; }

        [Range(0, 99999)]
        [Required]
        [Display(Name = "ZIP Code")]
        public int Zip { get; set; }

        [RegularExpression(@"\d{3}-\d{3}-\d{4}", ErrorMessage = "The phone number must be in the format ###-###-####")]
        [Required]
        [Display(Name = "Phone Number")]
        public string Phone { get; set; }
    }
}
