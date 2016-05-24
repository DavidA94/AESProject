using AES.Shared;
using AES.Shared.Contracts;
using AES.Web.Authorization;
using AES.Web.ManagementService;
using AES.Web.Models;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web.Mvc;

namespace AES.Web.Controllers
{
    [AESAuthorize(BadRedirectURL = "/EmployeeLogin", Role = EmployeeRole.ITSpecialist)]
    public class ITController : Controller
    {
        public ActionResult Dashboard()
        {
            return View();
        }

        #region Stores

        /// <summary>
        /// Gets the main Stores view
        /// </summary>
        public ActionResult Stores()
        {
            using (var m = new ManagementSvcClient())
            {
                return View(ConvertContractToModel(m.GetStoreList()));
            }
        }

        /// <summary>
        /// Deactivates the given store
        /// </summary>
        [HttpPost]
        public ActionResult DeactivateStore(StoreModel store)
        {
            string errorMessage = "";
            if (isBadModel(ModelState, ref errorMessage))
            {
                return Content(errorMessage);
            }

            using (var m = new ManagementSvcClient())
            {
                if(m.UpdateStoreInfo(ConvertModelToContract(store, false)))
                {
                    return Content("Success");
                }
                else
                {
                    return Content("Error");
                }
            }
        }

        /// <summary>
        /// Gets the list of stores that are active
        /// </summary>
        public ActionResult GetStoreList()
        {
            using (var m = new ManagementSvcClient())
            {
                // Get the use user list and then pass it to the partial view to be returned
                var stores = m.GetStoreList();
                return PartialView("_StoresList", ConvertContractToModel(stores));
            }
        }

        /// <summary>
        /// Creates a new store
        /// </summary>
        [HttpPost]
        public ActionResult NewStore(StoreModel store)
        {
            string errorMessage = "";
            if(isBadModel(ModelState, ref errorMessage)) {
                return Content(errorMessage);
            }

            using (var m = new ManagementSvcClient())
            {
                // Create a new user. If it's successful, return "Success"
                if (m.CreateNewStore(ConvertModelToContract(store)))
                {
                    return Content("Success:");
                }
                else
                {
                    return Content("Error");
                }
            }
        }

        /// <summary>
        /// Updates the given store
        /// </summary>
        [HttpPost]
        public ActionResult SaveStore(StoreModel store)
        {
            string errorMessage = "";
            if (isBadModel(ModelState, ref errorMessage))
            {
                return Content(errorMessage);
            }

            using (var m = new ManagementSvcClient())
            {
                // If it works, return "Success", "Error" otherwise
                if (m.UpdateStoreInfo(ConvertModelToContract(store)))
                {
                    return Content("Success");
                }
                else
                {
                    return Content("Error");
                }
            }
        }

        #endregion

        #region Users
        
        /// <summary>
        /// Shows the main Users page
        /// </summary>
        public ActionResult Users()
        {
            using (var m = new ManagementSvcClient())
            {
                return View(ConvertContractToModel(m.GetUserList(), m.GetStoreList()));
            }
        }

        /// <summary>
        /// Deactivates the given user
        /// </summary>
        [HttpPost]
        public ActionResult DeactivateUser(EmployeeUserModel e)
        {
            string errorMessage = "";
            if (isBadModel(ModelState, ref errorMessage))
            {
                return Content(errorMessage);
            }

            using (var m = new ManagementSvcClient())
            {
                // Set the role to be deactivated, and then just "Update" the user
                e.Role = EmployeeRole.Deactivated;
                if (m.UpdateUserInfo(ConvertModelToContract(e)))
                {
                    return Content("Success");
                }
                else
                {
                    return Content("Error");
                }
            }
        }

        /// <summary>
        /// Gets the list of users that are active
        /// </summary>
        public ActionResult GetUserList()
        {
            using (var m = new ManagementSvcClient())
            {
                // Get the use user list and then pass it to the partial view to be returned
                var users = m.GetUserList();
                return PartialView("_UsersList", ConvertContractToModel(users, m.GetStoreList()));
            }
        }

        /// <summary>
        /// Creates a new user
        /// </summary>
        [HttpPost]
        public ActionResult NewUser(EmployeeUserModel e)
        {
            string errorMessage = "";
            if (isBadModel(ModelState, ref errorMessage))
            {
                return Content(errorMessage);
            }

            using (var m = new ManagementSvcClient())
            {
                // Make a random password
                string newPassword = Path.GetRandomFileName().Substring(0, 8);

                // Create a new user. If it's successful, return "Success:[newPassword].
                if (m.CreateNewUser(ConvertModelToContract(e), newPassword))
                {
                    return Content("Success:" + newPassword);
                }
                else
                {
                    return Content("Error");
                }
            }
        }

        /// <summary>
        /// Updates the given user
        /// </summary>
        [HttpPost]
        public ActionResult SaveUser(EmployeeUserModel e)
        {
            string errorMessage = "";
            if (isBadModel(ModelState, ref errorMessage))
            {
                return Content(errorMessage);
            }

            using ( var m = new ManagementSvcClient())
            {
                // If it works, return "Success", "Error" otherwise
                if (m.UpdateUserInfo(ConvertModelToContract(e)))
                {
                    return Content("Success");
                }
                else
                {
                    return Content("Error");
                }
            }
        }

        #endregion

        #region Converters

        private IEnumerable<EmployeeUserModel> ConvertContractToModel(IEnumerable<EmployeeUserContract> data, IEnumerable<StoreContract> storeList)
        {
            var retList = new List<EmployeeUserModel>();

            foreach(var user in data)
            {
                retList.Add(new EmployeeUserModel()
                {
                    AvailableStores = new SelectList(storeList, "StoreID", "Name", user.StoreID),
                    Email = user.Email,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Role = user.Role,
                    StoreID = user.StoreID,
                    StoreName = user.StoreName
                });
            }

            return retList;
        }
        
        private EmployeeUserContract ConvertModelToContract(EmployeeUserModel user)
        {
            return new EmployeeUserContract()
            {
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Role = user.Role,
                StoreID = user.StoreID,
                StoreName = user.StoreName,
            };
        }

        private IEnumerable<StoreModel> ConvertContractToModel(IEnumerable<StoreContract> stores)
        {
            var retList = new List<StoreModel>();

            foreach(var store in stores)
            {
                retList.Add(new StoreModel()
                {
                    Address = store.Address,
                    City = store.City,
                    Name = store.Name,
                    Phone = store.Phone,
                    State = store.State,
                    StoreID = store.StoreID,
                    Zip = store.Zip
                });
            }

            return retList;
        }

        private StoreContract ConvertModelToContract(StoreModel store, bool isActive = true)
        {
            return new StoreContract()
            {
                Address = store.Address,
                City = store.City,
                IsActive = isActive,
                Name = store.Name,
                Phone = store.Phone,
                State = store.State,
                StoreID = store.StoreID,
                Zip = store.Zip
            };
        }

        #endregion

        #region Helper Methods

        /// <summary>
        /// Indates if the model passed in has bad data
        /// </summary>
        /// <param name="model">The model to look at</param>
        /// <param name="errorMessage">Where the error message is put if there is one</param>
        /// <returns>True if there aer any errors, false otherwise</returns>
        private bool isBadModel(ModelStateDictionary model, ref string errorMessage)
        {
            errorMessage = "";
            if (!ModelState.IsValid)
            {
                foreach (var error in ModelState.Where(m => m.Value.Errors.Count > 0))
                {
                    foreach (var e in error.Value.Errors)
                    {
                        errorMessage += e.ErrorMessage + "\n";
                    }
                }
            }

            return !ModelState.IsValid;
        }

        #endregion
    }
}