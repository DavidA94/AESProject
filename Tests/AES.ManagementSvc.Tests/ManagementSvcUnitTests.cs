using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using AES.Shared;
using AES.Entities.Contexts;
using System.Linq;
using AES.Entities.Tables;
using AES.Shared.Contracts;

namespace AES.ManagementSvc.Tests
{
    [TestClass]
    public class ManagementSvcUnitTests
    {
        private const string FIRST_NAME = "BillyBob";
        private const string LAST_NAME = "Jones";
        private const int storeID = 1;
        private const EmployeeRole role = EmployeeRole.ITSpecialist;

        public ManagementSvcUnitTests()
        {
            DBFileManager.SetDataDirectory(true);
        }

        [TestMethod]
        public void ManagementSvc_GetUserList()
        {
            // Using seeded data
            var m = new ManagementSvc();

            var users = m.GetUserList();

            Assert.IsTrue(users.Length == 1);

            var user = users[0];

            using (var db = new AESDbContext())
            {
                var seededUser = db.EmployeeUsers.FirstOrDefault();

                Assert.AreEqual(user.Email, seededUser.Email);
                Assert.AreEqual(user.FirstName, seededUser.FirstName);
                Assert.AreEqual(user.LastName, seededUser.LastName);
                Assert.AreEqual(user.MustResetPassword, seededUser.MustResetPassword);
                Assert.AreEqual(user.Role, seededUser.Role);
                Assert.AreEqual(user.StoreID, seededUser.StoreID);
            }
        }

        [TestMethod]
        public void ManagementSvc_UpdateUser()
        {
            // Using the seeded user

            EmployeeUser user;
            EmployeeRole originalRole;
            using (var db = new AESDbContext())
            {
                user = db.EmployeeUsers.FirstOrDefault();
                originalRole = user.Role;
            }

            var m = new ManagementSvc();
            var updateValid1 = m.UpdateUserInfo(new EmployeeUserContract()
            {
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Role = user.Role,
                StoreID = user.StoreID
            });

            var updateValid2 = m.UpdateUserInfo(new EmployeeUserContract()
            {
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Role = EmployeeRole.StoreManager,
                StoreID = user.StoreID
            });

            var restoreRole = m.UpdateUserInfo(new EmployeeUserContract()
            {
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Role = originalRole,
                StoreID = user.StoreID
            });

            var badStoreID = m.UpdateUserInfo(new EmployeeUserContract()
            {
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Role = EmployeeRole.StoreManager,
                StoreID = -1
            });

            var mismatchName = m.UpdateUserInfo(new EmployeeUserContract()
            {
                Email = user.Email,
                FirstName = "BillyBob",
                LastName = user.LastName,
                Role = EmployeeRole.StoreManager,
                StoreID = user.StoreID
            });

            Assert.IsTrue(updateValid1);
            Assert.IsTrue(updateValid2);
            Assert.IsTrue(restoreRole);
            Assert.IsFalse(badStoreID);
            Assert.IsFalse(mismatchName);

        }

        [TestMethod]
        public void ManagementSvc_GetStoreList()
        {
            // Using seeded data

            var m = new ManagementSvc();
            var stores = m.GetStoreList();

            Assert.IsTrue(stores.Length >= 2);

            using (var db = new AESDbContext())
            {
                var dbStores = db.Stores.Take(2);

                foreach(var store in dbStores)
                {
                    var mStore = stores.FirstOrDefault(s => s.StoreID == store.ID);
                    Assert.IsNotNull(mStore);

                    Assert.AreEqual(store.Address, mStore.Address);
                    Assert.AreEqual(store.City, mStore.City);
                    Assert.AreEqual(store.Name, mStore.Name);
                    Assert.AreEqual(store.Phone, mStore.Phone);
                    Assert.AreEqual(store.State, mStore.State);
                    Assert.AreEqual(store.Zip, mStore.Zip);
                }
            }
        }

        [TestMethod]
        public void ManagementSvc_UpdateStore()
        {
            const string NEW_NAME = "Abbra Kadabra Store!";

            // Get seeded stores
            var m = new ManagementSvc();
            var stores = m.GetStoreList();

            Assert.IsTrue(stores.Length >= 2);

            var storeToMessWith = new StoreContract()
            {
                Address = stores[0].Address,
                City = stores[0].City,
                Name = stores[0].Name,
                IsActive = stores[0].IsActive,
                Phone = stores[0].Phone,
                State = stores[0].State,
                StoreID = stores[0].StoreID,
                Zip = stores[0].Zip
            };

            storeToMessWith.Address = stores[1].Address;
            var badAddress = m.UpdateStoreInfo(storeToMessWith);

            storeToMessWith.Address = stores[0].Address;
            storeToMessWith.Name = stores[1].Name;
            var badName = m.UpdateStoreInfo(storeToMessWith);

            storeToMessWith.Name = stores[0].Name;
            storeToMessWith.Phone = stores[1].Phone;
            var badPhone = m.UpdateStoreInfo(storeToMessWith);

            storeToMessWith.Phone = stores[0].Phone;
            storeToMessWith.Name = NEW_NAME;
            var goodUpdate1 = m.UpdateStoreInfo(storeToMessWith);

            var editedStore = m.GetStoreList().FirstOrDefault(s => s.StoreID == storeToMessWith.StoreID);

            storeToMessWith.Name = stores[0].Name;
            var goodUpdate2 = m.UpdateStoreInfo(storeToMessWith);
            var revertedStore = m.GetStoreList().FirstOrDefault(s => s.StoreID == storeToMessWith.StoreID);

            Assert.IsFalse(badAddress);
            Assert.IsFalse(badName);
            Assert.IsFalse(badPhone);
            Assert.IsTrue(goodUpdate1);
            Assert.IsTrue(goodUpdate2);

            Assert.IsNotNull(editedStore);
            Assert.IsNotNull(revertedStore);

            Assert.AreEqual(editedStore.Address, stores[0].Address);
            Assert.AreEqual(editedStore.City, stores[0].City);
            Assert.AreEqual(editedStore.Name, NEW_NAME);
            Assert.AreEqual(editedStore.Phone, stores[0].Phone);
            Assert.AreEqual(editedStore.State, stores[0].State);
            Assert.AreEqual(editedStore.Zip, stores[0].Zip);

            Assert.AreEqual(revertedStore.Address, stores[0].Address);
            Assert.AreEqual(revertedStore.City, stores[0].City);
            Assert.AreEqual(revertedStore.Name, stores[0].Name);
            Assert.AreEqual(revertedStore.Phone, stores[0].Phone);
            Assert.AreEqual(revertedStore.State, stores[0].State);
            Assert.AreEqual(revertedStore.Zip, stores[0].Zip);
        }

        private void removeUser()
        {
            using (var db = new AESDbContext())
            {
                var user = db.EmployeeUsers.FirstOrDefault(e => e.FirstName == FIRST_NAME && e.LastName == e.LastName);
                if (user != null)
                {
                    db.EmployeeUsers.Remove(user);
                    db.SaveChanges();
                }
            }
        }
    }
}
