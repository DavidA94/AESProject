using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        [Range(10000, 99999)]
        public int Zip { get; set; }

        [RegularExpression(@"\d{3}-\d{3}-\d{4}")]
        public string Phone { get; set; }

        public decimal SalaryExpectation { get; set; }
    }
}
