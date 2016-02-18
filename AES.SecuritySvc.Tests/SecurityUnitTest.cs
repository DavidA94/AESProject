using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using AES.SecuritySvc.Contracts;

namespace AES.SecuritySvc.Tests
{
    [TestClass]
    public class SecurityUnitTest
    {
        [TestMethod]
        public void CheckLogin()
        {
            var s = new Security();

            var valid = s.IsValidUser(new UserLoginInfo()
            {
                FirstName = "David",
                LastName = "Antonucci",
                DOB = new DateTime(1994, 12, 15),
                SSN = "123-45-6789"
            });

            Assert.IsTrue(valid);
        }
    }
}
