using AES.Entities.Contexts;
using AES.Entities.Tables;
using AES.SecuritySvc.Contracts;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;

namespace AES.SecuritySvc.Tests
{
    [TestClass]
    public class SecurityUnitTest
    {
        private const string FIRSTNAME = "TestFirst";
        private const string LASTNAME = "TestLast";
        private const string SSN = "11-22-3344";
        private readonly string SSN_CRYPT;
        private readonly DateTime TEST_DOB;

        public SecurityUnitTest()
        {
            SSN_CRYPT = Encryption.Encrypt(SSN);
            TEST_DOB = new DateTime(1964, 6, 2);
        }

        [TestMethod]
        public void TC3_UserExists()
        {
            using (var db = new ApplicantDbContext())
            {
                // Holds the user
                ApplicantUser user = null;

                // Ensure the user is in the DB
                userInDB(ref user, db, true);

                // Try to login via the security module
                var s = new SecuritySvc();
                var validUser = s.ValidateUser(new ApplicantInfoContract(FIRSTNAME, LASTNAME, SSN, TEST_DOB));

                Assert.AreEqual(user.userID, validUser.UserID);
            }
        }

        [TestMethod]
        public void TC3_NewUser()
        {
            // Get the context
            using (var db = new ApplicantDbContext())
            {
                // Holds the user
                ApplicantUser user = null;

                // Ensure the user is not in the DB
                userInDB(ref user, db, false);

                // Try to login via the security module
                var s = new SecuritySvc();
                var newUser = s.ValidateUser(new ApplicantInfoContract(FIRSTNAME, LASTNAME, SSN, TEST_DOB));

                Assert.IsNotNull(newUser);
                Assert.AreEqual(newUser.FirstName, FIRSTNAME);
                Assert.AreEqual(newUser.LastName, LASTNAME);
                Assert.AreEqual(newUser.DOB, TEST_DOB);
            }
        }

        [TestMethod]
        public void TC3_BadCredentials()
        {
            using (var db = new ApplicantDbContext())
            {

                // Holds the user
                ApplicantUser user = null;

                // Ensure the user is in the DB
                userInDB(ref user, db, true);

                // Try to log in with bad credentials
                var s = new SecuritySvc();
                var badFName = s.ValidateUser(new ApplicantInfoContract(FIRSTNAME.Remove(FIRSTNAME.Length - 2), LASTNAME, SSN, TEST_DOB));
                var badLName = s.ValidateUser(new ApplicantInfoContract(FIRSTNAME, LASTNAME.Remove(LASTNAME.Length - 2), SSN, TEST_DOB));
                var badDOB = s.ValidateUser(new ApplicantInfoContract(FIRSTNAME, LASTNAME, SSN, TEST_DOB.AddDays(1)));

                Assert.IsNull(badFName);
                Assert.IsNull(badLName);
                Assert.IsNull(badDOB);
            }
        }

        [TestMethod]
        public void TC3_IncompleteCredentials()
        {
            using (var db = new ApplicantDbContext())
            {
                // Try to log in with incomplete credentials
                var s = new SecuritySvc();
                var badFName = s.ValidateUser(new ApplicantInfoContract(null, LASTNAME, SSN, TEST_DOB));
                var badLName = s.ValidateUser(new ApplicantInfoContract(FIRSTNAME, null, SSN, TEST_DOB));
                var badSSN = s.ValidateUser(new ApplicantInfoContract(FIRSTNAME, LASTNAME, null, TEST_DOB));

                // Can't have null DateTime object
                // var badDOB = s.ValidateUser(new ApplicantInfoContract(FIRSTNAME, LASTNAME, SSN, null));

                Assert.IsNull(badFName);
                Assert.IsNull(badLName);
                Assert.IsNull(badSSN);
            }
        }


        private void userInDB(ref ApplicantUser user, ApplicantDbContext db, bool isIn)
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
                            DOB = TEST_DOB,
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
