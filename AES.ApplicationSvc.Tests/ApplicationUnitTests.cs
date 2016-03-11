using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using AES.Entities.Contexts;
using System.Linq;

namespace AES.ApplicationSvc.Tests
{
    [TestClass]
    public class ApplicationUnitTests
    {
        [TestMethod]
        public void TC10_SaveSinglePartialApplication()
        {
            using (var db = new ApplicationDbContext())
            {
                //db.Applications.FirstOrDefault(a => a.)
            }
        }
    }
}
