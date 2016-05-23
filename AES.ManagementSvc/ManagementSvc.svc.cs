using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using AES.Entities.Tables;
using AES.Shared;
using AES.Entities.Contexts;
using AES.Shared.Contracts;
using AES.ManagementSvc.SecurityService;
using System.IO;
using System.Text.RegularExpressions;

namespace AES.ManagementSvc
{
    public class ManagementSvc : IManagementSvc
    {
        public ManagementSvc()
        {
            DBFileManager.SetDataDirectory();
        }

        #region Stores

        public bool CreateNewStore(StoreContract store)
        {
            using (var db = new AESDbContext())
            {
                // Ensure there's no store with the same name, address, or phone number
                var existingStore = db.Stores.FirstOrDefault(s => s.Address.ToLower() == store.Address.ToLower() ||
                                                                  s.Name.ToLower() == store.Name.ToLower() ||
                                                                  s.Phone.ToLower() == store.Phone.ToLower());

                if(existingStore != null)
                {
                    return false;
                }

                db.Stores.Add(new Store()
                {
                    Address = store.Address,
                    City = store.City,
                    IsActive = true,
                    Name = store.Name,
                    Phone = store.Phone,
                    State = store.State,
                    Zip = store.Zip
                });

                try
                {
                    if(db.SaveChanges() > 0)
                    {
                        return true;
                    }
                    return false;
                }
                catch
                {
                    return false;
                }
            }
        }

        public StoreContract[] GetStoreList()
        {
            using (var db = new AESDbContext())
            {
                return ConvertTableToContract(db.Stores.Where(s => s.IsActive)).ToArray();
            }
        }

        public bool UpdateStoreInfo(StoreContract store)
        {
            using (var db = new AESDbContext())
            {
                var dbStore = db.Stores.FirstOrDefault(s => s.ID == store.StoreID);
                
                // Ensure there's no store with the same name, address, or phone number
                var count = db.Stores.Count(s => s.Address.ToLower() == store.Address.ToLower() ||
                                            s.Name.ToLower() == store.Name.ToLower() ||
                                            s.Phone.ToLower() == store.Phone.ToLower());


                if (dbStore == null || count > 1) {
                    return false;
                }

                dbStore.Address = store.Address;
                dbStore.City = store.City;
                dbStore.Name = store.Name;
                dbStore.IsActive = store.IsActive;
                dbStore.Phone = store.Phone;
                dbStore.State = store.State;
                dbStore.Zip = store.Zip;

                try
                {
                    db.SaveChanges();
                    return true;
                }
                catch
                {
                    return false;
                }
            }
        }

        #endregion

        #region Users

        public bool CreateNewUser(EmployeeUserContract employee, string password)
        {
            employee.MustResetPassword = true;

            var s = new SecuritySvcClient();
            return s.CreateEmployee(employee, password);
        }

        public EmployeeUserContract[] GetUserList()
        {
            using (var db = new AESDbContext())
            {
                return ConvertTableToContract(db.EmployeeUsers.Where(e => e.Role != EmployeeRole.Deactivated)).ToArray();
            }
        }

        public bool UpdateUserInfo(EmployeeUserContract user)
        {
            using (var db = new AESDbContext())
            {
                // If we can't find the store, then there is an error
                var store = db.Stores.FirstOrDefault(s => s.ID == user.StoreID);
                if (store == null)
                {
                    return false;
                }

                // Try to get the user so we know if we're updating or creating
                var dbUser = db.EmployeeUsers.FirstOrDefault(e => e.Email.ToLower() == user.Email.ToLower());

                // If we didn't find them, error out
                if (dbUser == null)
                {
                    return false;
                }
                // If we got them, ensure the first/last name match
                if (dbUser.FirstName.ToLower() != user.FirstName.ToLower() ||
                    dbUser.LastName.ToLower() != user.LastName.ToLower())
                {
                    return false;
                }

                // Update the store and role
                dbUser.Store = store;
                dbUser.Role = user.Role;

                // Save the changes.
                try
                {
                    db.SaveChanges();
                    return true;
                }
                catch
                {
                    return false;
                }
            }
        }

        #endregion
        

        #region Converters

        private List<EmployeeUserContract> ConvertTableToContract(IEnumerable<EmployeeUser> employees)
        {
            var retList = new List<EmployeeUserContract>();

            foreach (var e in employees)
            {
                retList.Add(new EmployeeUserContract()
                {
                    Email = e.Email,
                    FirstName = e.FirstName,
                    LastName = e.LastName,
                    Role = e.Role,
                    StoreID = e.StoreID,
                    StoreName = e.Store.Name
                });
            }

            return retList;
        }

        private List<StoreContract> ConvertTableToContract(IEnumerable<Store> stores)
        {
            var retList = new List<StoreContract>();

            foreach (var store in stores)
            {
                retList.Add(new StoreContract()
                {
                    Address = store.Address,
                    City = store.City,
                    IsActive = store.IsActive,
                    Name = store.Name,
                    Phone = store.Phone,
                    State = store.State,
                    StoreID = store.ID,
                    Zip = store.Zip
                });
            }

            return retList;
        }

        #endregion
    }
}
