using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AES.Entities.Tables
{
    public class UserInfo
    {
        public UserInfo()
        {
            DTCallStartTime = new DateTime(1970, 1, 1);
            DTCallEndTime = new DateTime(1970, 1, 1);
        }

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

        public decimal SalaryExpectation { get; set; }

        /// <summary>
        /// Do not use directly. Use [TimeSpan] CallStartTime.
        /// </summary>
        public DateTime DTCallStartTime { get; set; }

        /// <summary>
        /// Do not use directly. Use [TimeSpan] CallEndTime.
        /// </summary>
        public DateTime DTCallEndTime { get; set; }

        #region TimeSpan Start/Stop

        [NotMapped]
        public TimeSpan CallStartTime
        {
            get { return DateTimeHelper.ConvertDateTime(DTCallStartTime); }
            set { DTCallStartTime = DateTimeHelper.ConvertTimeSpan(value); }
        }

        [NotMapped]
        public TimeSpan CallEndTime
        {
            get { return DateTimeHelper.ConvertDateTime(DTCallEndTime); }
            set { DTCallEndTime = DateTimeHelper.ConvertTimeSpan(value); }
        }

        #endregion
    }
}
