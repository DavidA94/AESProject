using AES.Entities.Contexts;
using AES.Entities.Tables;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;

namespace AES.OpeningsSvc.Tests
{
    [TestClass]
    public class OpeningTests
    {

        private const string STORE1_ADDRESS = "123 Maple Street";
        private const string STORE1_CITY = "Thomasville";
        private const string STORE1_NAME = "AES Thomasville";
        private const string STORE1_STATE = "OR";
        private const string STORE1_PHONE = "321-546-2545";
        private const int STORE1_ZIP = 12345;

        private const string STORE2_ADDRESS = "456 Arbor Avenue";
        private const string STORE2_CITY = "Georgeville";
        private const string STORE2_NAME = "AES Georgeville";
        private const string STORE2_STATE = "WA";
        private const string STORE2_PHONE = "123-456-9780";
        private const int STORE2_ZIP = 23456;

        private const string JOB1_DESC_SHORT = "Test Job 1";
        private const string JOB1_DESC_LONG = "The long description for test job 1";

        private const string JOB2_DESC_SHORT = "Test Job 2";
        private const string JOB2_DESC_LONG = "The long description for test job 2";

        [TestMethod]
        public void TC_Openings()
        {

            using (var db = new OpeningDbContext())
            {

                Store TestStore1 = new Store()
                {
                    Address = STORE1_ADDRESS,
                    City = STORE1_CITY,
                    Name = STORE1_NAME,
                    State = STORE1_STATE,
                    Zip = STORE1_ZIP,
                    Phone = STORE1_PHONE
                };

                Store TestStore2 = new Store()
                {
                    Address = STORE2_ADDRESS,
                    City = STORE2_CITY,
                    Name = STORE2_NAME,
                    State = STORE2_STATE,
                    Zip = STORE2_ZIP,
                    Phone = STORE2_PHONE
                };

                Job TestJob1 = new Job()
                {
                    descLong = JOB1_DESC_LONG,
                    descShort = JOB1_DESC_SHORT
                };

                Job TestJob2 = new Job()
                {
                    descLong = JOB2_DESC_LONG,
                    descShort = JOB2_DESC_SHORT
                };

                db.JobOpenings.RemoveRange(db.JobOpenings);
                db.Stores.RemoveRange(db.Stores);
                db.Jobs.RemoveRange(db.Jobs);

                db.Stores.Add(TestStore1);
                db.Stores.Add(TestStore2);
                db.Jobs.Add(TestJob1);
                db.Jobs.Add(TestJob2);
                db.JobOpenings.Add
                (
                    new JobOpening()
                    {
                        Store = TestStore1,
                        Job = TestJob1
                    }    
                );
                db.JobOpenings.Add
                (
                    new JobOpening()
                    {
                        Store = TestStore2,
                        Job = TestJob2
                    }
                );
                db.JobOpenings.Add
                (
                    new JobOpening()
                    {
                        Store = TestStore1,
                        Job = TestJob1
                    }
                );
                db.JobOpenings.Add
                (
                    new JobOpening()
                    {
                        Store = TestStore2,
                        Job = TestJob2
                    }
                );
                db.SaveChanges();

                var gottenOpenings = db.JobOpenings.Where(opening => opening.Store.ID == TestStore1.ID).ToList();

                OpeningSvc openingService = new OpeningSvc();

                var Store1Openings = openingService.GetOpenings(TestStore1.ID);
                var Store2Openings = openingService.GetOpenings(TestStore2.ID);

                foreach (var opening in Store1Openings)
                {
                    Assert.AreEqual(TestJob1.descShort, opening.shortDesc);
                }

                foreach (var opening in Store2Openings)
                {
                    Assert.AreEqual(TestJob2.descShort, opening.shortDesc);
                }

            }
        }
    }
}
