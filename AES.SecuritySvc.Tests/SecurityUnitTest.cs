using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using AES.SecuritySvc.Contracts;
using AES.Entities.Contexts;
using System.Linq;
using AES.Entities.Tables;

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
            var c = new SimpleCrypto.PBKDF2();
            SSN_CRYPT = c.Compute(SSN, Security.SSNSALT);
            DOB = new DateTime(1970, 1, 1);
        }

        [TestMethod]
        public void TC3_UserExists()
        {


            using (var db = new ApplicantDbContext())
            {
                // Holds the user
                ApplicantUser user = null;

                // If it doesn't work the first time getting it, then we will add it in and do it again.
                for (int i = 0; i < 2; ++i)
                {
                    user = db.ApplicantUsers.FirstOrDefault(u => u.SSN == SSN_CRYPT);

                    if (user == null && i == 0)
                    {
                        db.ApplicantUsers.Add(new ApplicantUser()
                        {
                            FirstName = FIRSTNAME,
                            LastName = LASTNAME,
                            DOB = DOB,
                            SSN = SSN_CRYPT,
                            UserInfo = new UserInfo()
                        });
                        db.SaveChanges();
                    }
                    else if(user == null && i != 0)
                    {
                        Assert.IsTrue(false);
                    }
                    else break;
                }

                // Try to login via the security module
                Security s = new Security();
                var validUser = s.ValidateUser(new UserInfoContract(FIRSTNAME, LASTNAME, SSN, DOB));

                Assert.AreEqual(user.userID, validUser.UserID);
            }
        }
    }
}
