using AES.Entities.Contexts;
using AES.Entities.Tables;
using AES.Shared;
using AES.Shared.Contracts;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;

namespace AES.SecuritySvc.Tests
{
    [TestClass]
    public class SecurityUnitTest
    {
        private const string FIRSTNAME = "TestFirst";
        private const string LASTNAME = "TestLast";
        private const string SSN = "11-22-3344";
        private readonly string SSN_CRYPT;
        private readonly DateTime DOB;

        public SecurityUnitTest()
        {
            SSN_CRYPT = Encryption.Encrypt(SSN);
            DOB = new DateTime(1970, 6, 2);

            DBFileManager.SetDataDirectory(true);
        }

        [TestMethod]
        public void SecuritySvc_UserExists()
        {
            using (var db = new AESDbContext())
            {
                // Holds the user
                ApplicantUser user = null;

                // Ensure the user is in the DB
                userInDB(ref user, db, true);

                // Try to login via the security module
                var s = new SecuritySvc();
                var validUser = s.ValidateUser(new ApplicantInfoContract(FIRSTNAME, LASTNAME, SSN, DOB));

                Assert.AreEqual(user.userID, validUser.UserID);
            }
        }

        [TestMethod]
        public void SecuritySvc_NewUser()
        {
            // Get the context
            using (var db = new AESDbContext())
            {
                // Holds the user
                ApplicantUser user = null;

                // Ensure the user is not in the DB
                userInDB(ref user, db, false);

                // Try to login via the security module
                var s = new SecuritySvc();
                var newUser = s.ValidateUser(new ApplicantInfoContract(FIRSTNAME, LASTNAME, SSN, DOB));

                Assert.IsNotNull(newUser);
                Assert.AreEqual(newUser.FirstName, FIRSTNAME);
                Assert.AreEqual(newUser.LastName, LASTNAME);
                Assert.AreEqual(newUser.DOB, DOB);
            }
        }

        [TestMethod]
        public void SecuritySvc_BadCredentials()
        {
            using (var db = new AESDbContext())
            {

                // Holds the user
                ApplicantUser user = null;

                // Ensure the user is in the DB
                userInDB(ref user, db, true);

                // Try to log in with bad credentials
                var s = new SecuritySvc();
                var badFName = s.ValidateUser(new ApplicantInfoContract(FIRSTNAME.Remove(FIRSTNAME.Length - 2), LASTNAME, SSN, DOB));
                var badLName = s.ValidateUser(new ApplicantInfoContract(FIRSTNAME, LASTNAME.Remove(LASTNAME.Length - 2), SSN, DOB));
                var badDOB = s.ValidateUser(new ApplicantInfoContract(FIRSTNAME, LASTNAME, SSN, DOB.AddDays(1)));

                Assert.IsNull(badFName);
                Assert.IsNull(badLName);
                Assert.IsNull(badDOB);
            }
        }

        [TestMethod]
        public void SecuritySvc_IncompleteCredentials()
        {
            using (var db = new AESDbContext())
            {
                // Try to log in with incomplete credentials
                var s = new SecuritySvc();
                var badFName = s.ValidateUser(new ApplicantInfoContract(null, LASTNAME, SSN, DOB));
                var badLName = s.ValidateUser(new ApplicantInfoContract(FIRSTNAME, null, SSN, DOB));
                var badSSN = s.ValidateUser(new ApplicantInfoContract(FIRSTNAME, LASTNAME, null, DOB));

                // Can't have null DateTime object
                // var badDOB = s.ValidateUser(new ApplicantInfoContract(FIRSTNAME, LASTNAME, SSN, null));

                Assert.IsNull(badFName);
                Assert.IsNull(badLName);
                Assert.IsNull(badSSN);
            }
        }

        [TestMethod]
        public void SecuritySvc_CreateEmployee()
        {

            using (var db = new AESDbContext())
            {
                // Clear any old runs
                db.EmployeeUsers.RemoveRange(db.EmployeeUsers.Where(e => e.FirstName == FIRSTNAME && e.LastName == LASTNAME));
                db.SaveChanges();
            }
            using (var db = new AESDbContext())
            { 

                var s = new SecuritySvc();
                var store = db.Stores.FirstOrDefault();

                Assert.IsNotNull(store);
                var storeId = store.ID;

                var cryptRandom = new RNGCryptoServiceProvider();
                var intRandom = new Random(Guid.NewGuid().GetHashCode());

                var invalidPassEmployee = new EmployeeUserContract()
                {
                    FirstName = "Random",
                    LastName = "Davis",
                    Role = EmployeeRole.StoreManager,
                    StoreID = storeId
                };

                for (var i = 0; i < 20; i++)
                {
                    var randomBytes = new byte[intRandom.Next(1, 30)];
                    cryptRandom.GetNonZeroBytes(randomBytes);
                    var normalPass = Convert.ToBase64String(randomBytes);
                    var newValidEmployee = new EmployeeUserContract
                    {
                        FirstName = FIRSTNAME,
                        LastName = LASTNAME,
                        Role = EmployeeRole.HqQStaffingExpert,
                        StoreID = storeId
                    };

                    Assert.IsTrue(s.CreateEmployee(newValidEmployee, normalPass));

                    // Get what the email will be
                    string email;
                    using (var dbRefresh = new AESDbContext())
                    {
                        int count = dbRefresh.EmployeeUsers.Count(e => e.FirstName == FIRSTNAME && e.LastName == LASTNAME);
                        email = string.Format("{0}.{1}{2}@AES.com", FIRSTNAME, LASTNAME, count == 1 ? "" : count.ToString());
                    }

                    var credentials = new EmployeeCredentialsContract() { Email = email, Password = normalPass };
                    var gottenEmployee = s.ValidateEmployeeUser(credentials);
                    Assert.IsNotNull(gottenEmployee);
                    Assert.AreEqual(gottenEmployee.FirstName, newValidEmployee.FirstName);
                    Assert.AreEqual(gottenEmployee.LastName, newValidEmployee.LastName);
                    Assert.AreEqual(gottenEmployee.Email, email);
                    Assert.AreEqual(gottenEmployee.Role, newValidEmployee.Role);
                    Assert.IsTrue(gottenEmployee.MustResetPassword);
                }

                Assert.IsFalse(s.CreateEmployee(invalidPassEmployee, ""));
                Assert.IsFalse(s.CreateEmployee(invalidPassEmployee, null));

                invalidPassEmployee.FirstName = "a@a.a";
                Assert.IsFalse(s.CreateEmployee(invalidPassEmployee, "Passw0rd!"));
            }

            // Clean up the users we created
            using (var db = new AESDbContext())
            {
                db.EmployeeUsers.RemoveRange(db.EmployeeUsers.Where(e => e.FirstName == FIRSTNAME && e.LastName == LASTNAME));
                db.SaveChanges();
            }

        }
        
        [TestMethod]
        public void SecuritySvc_EditEmployeePassword()
        {
            using (var db = new AESDbContext())
            {
                // Find the salted user
                var user = db.EmployeeUsers.FirstOrDefault(e => e.Email.ToLower() == "employee@aes.com");

                Assert.IsNotNull(user);

                var s = new SecuritySvc();
                Assert.IsTrue(s.UpdateUserPassword(new EmployeeCredentialsContract() { Email = "employee@aes.com", Password = "Omicron" }, "BrandNewPassword!"));
                Assert.IsTrue(s.UpdateUserPassword(new EmployeeCredentialsContract() { Email = "employee@aes.com", Password = "BrandNewPassword!" }, "Omicron"));
            }
        }

        [TestMethod]
        public void SecuritySvc_ForgotPassword()
        {
            using (var db = new AESDbContext())
            {
                // Get where the emails are being saved
                var emailDir = Directory.GetCurrentDirectory();

                // Ensure there are no emails in the directory
                foreach (var file in (new DirectoryInfo(emailDir)).EnumerateFiles("*.eml"))
                {
                    File.Delete(file.FullName);
                }

                // Backup the hash/salt so we can restore them
                var user = db.EmployeeUsers.FirstOrDefault(e => e.Email.ToLower() == "employee@aes.com");
                var hash = user.PasswordHash.Clone() as byte[];
                var salt = user.Salt.Clone() as byte[];

                // Try to "forget" the password
                var s = new SecuritySvc();
                Assert.IsTrue(s.ForgotPassword(new EmployeeCredentialsContract() { Email = user.Email }));

                // Ensure the hash and salt are different
                using (var db2 = new AESDbContext())
                {
                    var user2 = db2.EmployeeUsers.FirstOrDefault(e => e.Email.ToLower() == "employee@aes.com");
                    Assert.AreNotEqual(user2.PasswordHash, hash);
                    Assert.AreNotEqual(user2.Salt, salt);

                    // Revert the hash/salt
                    user2.PasswordHash = hash;
                    user2.Salt = salt;
                    db2.SaveChanges();
                }

                var fileinfo = new FileInfo(new DirectoryInfo(emailDir).EnumerateFiles("*.eml").First().FullName);
                var fileContents = File.ReadAllText(fileinfo.FullName);

                // Ensure some of the email is there (assume the rest is good)
                Assert.IsTrue(fileContents.Contains("<h3>Hello Employee User,</h3><p>We heard you needed to reset your"));
                Assert.IsTrue(fileContents.Contains("log in. After logging in, you must create a new password.</p><p><"));
            }
        }

        /*
        [TestMethod]
        public void SecuritySvc_Sanity()
        {
            var s = new SecuritySvcTestClient.SecuritySvcClient();
            var excepted = false;

            var fakeEmployeeCredentials = new EmployeeCredentialsContract()
            {
                Email ="yada@yada.blah",
                Password = "Qwerty123"
            };

            var fakeUserInfo = new UserInfoContract()
            {
                Address = "asdasd",
                City = "asdf",
                EndCallTime = new TimeSpan(23, 59, 59),
                Nickname = "asdsad",
                Phone = "333-222-1111",
                SalaryExpectation = 12,
                StartCallTime = new TimeSpan(0, 0, 0),
                State = "OR",
                Zip = 12432
            };

            var fakeApplicant = new ApplicantInfoContract()
            {
                Availability = new AvailabilityContract()
                {
                    
                },
                DOB = new DateTime(1970,1,1),
                FirstName = "asdsad",
                LastName = "sdsdsd",
                SSN = "234-56-7890",
                UserInfo = fakeUserInfo
            };

            var cryptRandom = new RNGCryptoServiceProvider();
            var intRandom = new Random(Guid.NewGuid().GetHashCode());
            var randomBytes = new byte[intRandom.Next(1, 30)];
            cryptRandom.GetNonZeroBytes(randomBytes);

            var fakeEmail = Convert.ToBase64String(randomBytes) + "@gmail.com";

            var fakeEmployee = new EmployeeUserContract()
            {
                Email = fakeEmail,
                FirstName = "First",
                LastName = "Last",
                Role = EmployeeRole.HiringManager,
                UserInfo = fakeUserInfo
            };

            try
            {
                s.CreateEmployee(fakeEmployee, "nS3CuR3_P4$$W0RD");
                s.ValidateUser(fakeApplicant);
                s.ValidateEmployeeUser(fakeEmployeeCredentials);
            }
            catch (Exception)
            {
                excepted = true;
            }

            s.Close();

            Assert.IsFalse(excepted);
        }
        */

        private void userInDB(ref ApplicantUser user, AESDbContext db, bool isIn)
        {
            // If it doesn't work the first time getting it, then we will add it in and do it again.
            for (int i = 0; i < 2; ++i)
            {
                user = db.ApplicantUsers.FirstOrDefault(u => u.SSN == SSN_CRYPT);

                if (user == null && i == 0)
                {
                    if (isIn)
                    {
                        db.ApplicantUsers.Add(new ApplicantUser()
                        {
                            FirstName = FIRSTNAME,
                            LastName = LASTNAME,
                            DOB = DOB,
                            SSN = SSN_CRYPT,
                            UserInfo = new UserInfo(),
                            Availability = new Availability()
                        });
                        db.SaveChanges();
                    }
                    else
                    {
                        return;
                    }
                }
                else if(user != null && !isIn)
                {

                    db.ApplicantUsers.Remove(user);
                    db.SaveChanges();
                    
                    // Do a weird error if they aren't removed
                    if(db.ApplicantUsers.FirstOrDefault(u => u.SSN == SSN_CRYPT) != null)
                    {
                        Assert.IsFalse(true);
                    }
                    else
                    {
                        return;
                    }
                }
                // If we finish the loop, do something weird so we know that the user couldn't be added
                else if (user == null && i != 0)
                {
                    Assert.IsTrue(false);
                }
                else break;
            }
        }
    }
}
