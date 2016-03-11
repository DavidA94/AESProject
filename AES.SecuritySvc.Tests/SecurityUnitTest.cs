using AES.Entities.Contexts;
using AES.Entities.Tables;
using AES.Shared.Contracts;
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
        private readonly DateTime DOB;
        private readonly TimeSpan START_CALL;
        private readonly TimeSpan END_CALL;

        public SecurityUnitTest()
        {
            SSN_CRYPT = Encryption.Encrypt(SSN);
            DOB = new DateTime(1970, 6, 2);
            START_CALL = new TimeSpan(17, 0, 0);
            END_CALL = new TimeSpan(20, 0, 0);
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
                var validUser = s.ValidateUser(new ApplicantInfoContract(FIRSTNAME, LASTNAME, SSN, DOB, START_CALL, END_CALL));

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
                var newUser = s.ValidateUser(new ApplicantInfoContract(FIRSTNAME, LASTNAME, SSN, DOB, START_CALL, END_CALL));

                Assert.IsNotNull(newUser);
                Assert.AreEqual(newUser.FirstName, FIRSTNAME);
                Assert.AreEqual(newUser.LastName, LASTNAME);
                Assert.AreEqual(newUser.DOB, DOB);
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
                var badFName = s.ValidateUser(new ApplicantInfoContract(FIRSTNAME.Remove(FIRSTNAME.Length - 2), LASTNAME, SSN, DOB, START_CALL, END_CALL));
                var badLName = s.ValidateUser(new ApplicantInfoContract(FIRSTNAME, LASTNAME.Remove(LASTNAME.Length - 2), SSN, DOB, START_CALL, END_CALL));
                var badDOB = s.ValidateUser(new ApplicantInfoContract(FIRSTNAME, LASTNAME, SSN, DOB.AddDays(1), START_CALL, END_CALL));

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
                var badFName = s.ValidateUser(new ApplicantInfoContract(null, LASTNAME, SSN, DOB, START_CALL, END_CALL));
                var badLName = s.ValidateUser(new ApplicantInfoContract(FIRSTNAME, null, SSN, DOB, START_CALL, END_CALL));
                var badSSN = s.ValidateUser(new ApplicantInfoContract(FIRSTNAME, LASTNAME, null, DOB, START_CALL, END_CALL));

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
                            DOB = DOB,
                            SSN = SSN_CRYPT,
                            UserInfo = new UserInfo(),
                            Availability = new Availability(),
                            CallStartTime = START_CALL,
                            CallEndTime = END_CALL
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
