using System.ComponentModel.DataAnnotations;

namespace AES.Entities.Tables
{
    public class UserInfo
    {
        [Key]
        [Required]
        public int UserInfoID { get; set; }

        [StringLength(25)]
        public string Nickname { get; set; }

        [StringLength(50)]
        public string Address { get; set; }

        [StringLength(30)]
        public string City { get; set; }

        [StringLength(2)]
        public string State { get; set; }

        [Range(0, 99999)]
        public int Zip { get; set; }

        [RegularExpression(@"\d{3}-\d{3}-\d{4}")]
        public string Phone { get; set; }

        public double SalaryExpectation { get; set; }
    }
}
