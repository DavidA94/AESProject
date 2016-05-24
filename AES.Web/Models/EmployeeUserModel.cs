using AES.Shared;
using AES.Web.ManagementService;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace AES.Web.Models
{
    public class EmployeeUserModel
    {
        private static IEnumerable<SelectListItem> m_storeList;

        public EmployeeUserModel() { }

        public EmployeeUserModel(IEnumerable<SelectListItem> storeList)
        {
            m_storeList = storeList;
        }

        [StringLength(50)]
        [DataType(DataType.EmailAddress)]
        [Display(Name = "Email")]
        [Required]
        public string Email { get; set; }

        [Required]
        [Display(Name = "Employee Role")]
        public EmployeeRole Role { get; set; }

        [Required]
        [StringLength(25)]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Required]
        [StringLength(28)]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        [Display(Name = "Store Name")]
        public string StoreName { get; set; }

        [Required]
        [Display(Name = "Store Name")]
        public int StoreID { get; set; }

        public IEnumerable<SelectListItem> AvailableStores
        {
            get
            {
                if (m_storeList == null)
                {
                    IManagementSvc m = new ManagementSvcClient();
                    m_storeList = new SelectList(m.GetStoreList(), "StoreID", "Name", "Select Store");
                    return m_storeList;
                }
                return m_storeList;
            }
            set
            {
                m_storeList = value;
            }
        }
    }
}
