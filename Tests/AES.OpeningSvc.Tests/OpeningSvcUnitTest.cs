using System;
using AES.Entities.Contexts;
using AES.Entities.Tables;
using AES.OpeningsSvc.Contracts;
using AES.Shared;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Data.Entity.Migrations;
using System.Linq;
using AES.Opening.Tests.OpeningSvcTestClient;

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

        private const string JOB1_TITLE = "Job 1 Title";
        private const string JOB1_DESC_SHORT = "Job 1 Short Description";
        private const string JOB1_DESC_LONG = "The long description for Job 1";

        private const string JOB2_TITLE = "Job 2 Title";
        private const string JOB2_DESC_SHORT = "Job 2 Short Description";
        private const string JOB2_DESC_LONG = "The long description for Job 2";

        public OpeningTests()
        {
            DBFileManager.SetDataDirectory(true);
        }

        [TestMethod]        
        public void OpeningSvc_GetOpenings()
        {

            using (var db = new AESDbContext())
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
                    Title = JOB1_TITLE,
                    LongDescription = JOB1_DESC_LONG,
                    ShortDescription = JOB1_DESC_SHORT
                };

                Job TestJob2 = new Job()
                {
                    Title = JOB2_TITLE,
                    LongDescription = JOB2_DESC_LONG,
                    ShortDescription = JOB2_DESC_SHORT
                };

                var testJob1 = new JobOpening()
                {
                    Job = TestJob1
                };
                testJob1.Store = TestStore1;
                var testJob2 = new JobOpening()
                {
                    Job = TestJob2
                };
                testJob2.Store = TestStore2;

                // If this doesn't work, it probably means the data is already in the database.
                try
                {
                    db.Stores.AddOrUpdate(TestStore1);
                    db.Stores.AddOrUpdate(TestStore2);
                    db.Jobs.AddOrUpdate(TestJob1);
                    db.Jobs.AddOrUpdate(TestJob2);
                    db.JobOpenings.AddOrUpdate(testJob1);
                    db.JobOpenings.AddOrUpdate(testJob2);
                    db.SaveChanges();
                }
                catch { }

                var gottenOpenings = db.JobOpenings.Where(opening => opening.Store.ID == TestStore1.ID).ToList();

                var s = new OpeningSvc();

                var Store1Openings = s.GetApprovedOpenings(TestStore1.ID);
                var Store2Openings = s.GetApprovedOpenings(TestStore2.ID);

                foreach (var opening in Store1Openings)
                {
                    Assert.AreEqual(TestJob1.ShortDescription, opening.ShortDescription);
                    Assert.AreEqual(TestJob1.LongDescription, opening.LongDescription);
                    Assert.AreEqual(TestJob1.Title, opening.title);
                }

                foreach (var opening in Store2Openings)
                {
                    Assert.AreEqual(TestJob2.ShortDescription, opening.ShortDescription);
                    Assert.AreEqual(TestJob2.LongDescription, opening.LongDescription);
                    Assert.AreEqual(TestJob2.Title, opening.title);
                }

            }
        }

        [TestMethod]
        public void OpeningSvc_RequestOpening()
        {
            var s = new OpeningSvc();
            using (var db = new AESDbContext())
            {
                var newStore = new Store()
                {
                    Name = "Test Store",
                    Address = "Test Address",
                    City = "Test City",
                    Phone = "123-456-7654",
                    State = "OR",
                    Zip = 88888
                };

                db.Stores.RemoveRange(db.Stores.Where(st => st.Name == newStore.Name));
                db.Stores.AddOrUpdate(newStore);
                db.SaveChanges();

                var newJob = new Job()
                {
                    Title = "Openings Test Job Title",
                    ShortDescription = "Openings Test Job Short Description",
                    LongDescription = "Openings Test Job Long Description"
                };

                db.Jobs.RemoveRange(db.Jobs.Where(j => j.Title == newJob.Title));
                db.Jobs.AddOrUpdate(newJob);
                db.SaveChanges();

                string notes = "Test Request Notes";

                var openingRequest = new JobOpeningContract()
                {
                    JobID = newJob.JobID,
                    LongDescription = newJob.LongDescription,
                    ShortDescription = newJob.ShortDescription,
                    title = newJob.Title,
                    RequestNotes = notes
                };

                Assert.IsTrue(s.RequestOpenings(newStore.ID, openingRequest));
                Assert.AreEqual(1, db.JobOpenings.Count(o => o.Status == OpeningStatus.PENDING_APPROVAL && o.Job.JobID == newJob.JobID));
                Assert.IsTrue(s.RequestOpenings(newStore.ID, openingRequest));
                Assert.AreEqual(2, db.JobOpenings.Count(o => o.Status == OpeningStatus.PENDING_APPROVAL && o.Job.JobID == newJob.JobID));

                var pendingOpenings = s.GetPendingOpenings(newStore.ID);
                var allOpenings = s.GetAllOpenings(newStore.ID);
                var approvedOpenings = s.GetApprovedOpenings(newStore.ID);
                var rejectedOpenings = s.GetRejectedOpenings(newStore.ID);

                Assert.AreEqual(2, pendingOpenings.Count());
                Assert.AreEqual(2, allOpenings.Count());
                Assert.AreEqual(0, approvedOpenings.Count());
                Assert.AreEqual(0, rejectedOpenings.Count());

                db.JobOpenings.RemoveRange(db.JobOpenings.Where(o => o.Job.JobID == newJob.JobID));
                db.Jobs.RemoveRange(db.Jobs.Where(j => j.JobID == newJob.JobID));
                db.Stores.RemoveRange(db.Stores.Where(st => st.ID == newStore.ID));
                db.SaveChanges();
            }
        }

        [TestMethod]
        public void OpeningsSvc_Reject()
        {
            var s = new OpeningSvc();
            using (var db = new AESDbContext())
            {
                var newStore = new Store()
                {
                    Name = "Teasdasdsadst Store",
                    Address = "Test Aasdasdddress",
                    City = "Test Casdsadity",
                    Phone = "123-456-7654",
                    State = "OR",
                    Zip = 88888
                };

                db.Stores.RemoveRange(db.Stores.Where(st => st.Name == newStore.Name));
                db.Stores.AddOrUpdate(newStore);
                db.SaveChanges();

                var newJob = new Job()
                {
                    Title = "Openings Tesasdasdt Job Title",
                    ShortDescription = "Openiasdsadngs Test Job Short Description",
                    LongDescription = "Openings Test Job Long Descriasdasdption"
                };

                db.Jobs.RemoveRange(db.Jobs.Where(j => j.Title == newJob.Title));
                db.Jobs.AddOrUpdate(newJob);
                db.SaveChanges();

                string notes = "Test Requesasdasdt Notes";

                var openingRequest = new JobOpeningContract()
                {
                    JobID = newJob.JobID,
                    LongDescription = newJob.LongDescription,
                    ShortDescription = newJob.ShortDescription,
                    title = newJob.Title,
                    RequestNotes = notes
                };

                string rejectNotes = "You don't need another opening for that position";

                Assert.IsTrue(s.RequestOpenings(newStore.ID, openingRequest));
                Assert.AreEqual(1, db.JobOpenings.Count(o => o.Status == OpeningStatus.PENDING_APPROVAL && o.Job.JobID == newJob.JobID));
                var gottenOpeningRequest = new JobOpeningContract(db.JobOpenings.FirstOrDefault(jo => jo.Job.JobID == newJob.JobID));

                Assert.IsTrue(s.RejectOpening(gottenOpeningRequest, rejectNotes));
                Assert.AreEqual(1, db.JobOpenings.Count(o => o.Status == OpeningStatus.REJECTED && o.Job.JobID == newJob.JobID && o.StoreManagerNotes == rejectNotes));
                Assert.AreEqual(0, db.JobOpenings.Count(o => o.Status == OpeningStatus.PENDING_APPROVAL && o.Job.JobID == newJob.JobID));
                Assert.AreEqual(0, db.JobOpenings.Count(o => o.Status == OpeningStatus.APPROVED && o.Job.JobID == newJob.JobID));

                var pendingOpenings = s.GetPendingOpenings(newStore.ID);
                var allOpenings = s.GetAllOpenings(newStore.ID);
                var approvedOpenings = s.GetApprovedOpenings(newStore.ID);
                var rejectedOpenings = s.GetRejectedOpenings(newStore.ID);

                Assert.AreEqual(0, pendingOpenings.Count());
                Assert.AreEqual(1, allOpenings.Count());
                Assert.AreEqual(0, approvedOpenings.Count());
                Assert.AreEqual(1, rejectedOpenings.Count());

                db.JobOpenings.RemoveRange(db.JobOpenings.Where(o => o.Job.JobID == newJob.JobID));
                db.Jobs.RemoveRange(db.Jobs.Where(j => j.JobID == newJob.JobID));
                db.Stores.RemoveRange(db.Stores.Where(st => st.ID == newStore.ID));
                db.SaveChanges();
            }
        }

        [TestMethod]
        public void OpeningsSvc_Approve()
        {
            var s = new OpeningSvc();
            using (var db = new AESDbContext())
            {
                var newStore = new Store()
                {
                    Name = "Test Stasdsadore",
                    Address = "Test Adasdasddress",
                    City = "Tesasdsadt City",
                    Phone = "123-456-7654",
                    State = "OR",
                    Zip = 88888
                };

                db.Stores.RemoveRange(db.Stores.Where(st => st.Name == newStore.Name));
                db.Stores.AddOrUpdate(newStore);
                db.SaveChanges();

                var newJob = new Job()
                {
                    Title = "Oasdasdpenings Test Job Title",
                    ShortDescription = "Openinasdasdgs Test Job Short Description",
                    LongDescription = "Openiasdasdngs Test Job Long Description"
                };

                db.Jobs.RemoveRange(db.Jobs.Where(j => j.Title == newJob.Title));
                db.Jobs.AddOrUpdate(newJob);
                db.SaveChanges();

                string notes = "Test Reasdasdquest Notes";

                var openingRequest = new JobOpeningContract()
                {
                    JobID = newJob.JobID,
                    LongDescription = newJob.LongDescription,
                    ShortDescription = newJob.ShortDescription,
                    title = newJob.Title,
                    RequestNotes = notes
                };

                string approveNotes = "Yes, we should have an opening for this job";

                Assert.IsTrue(s.RequestOpenings(newStore.ID, openingRequest));
                Assert.AreEqual(1, db.JobOpenings.Count(o => o.Status == OpeningStatus.PENDING_APPROVAL && o.Job.JobID == newJob.JobID));

                var gottenOpeningRequest = new JobOpeningContract(db.JobOpenings.FirstOrDefault(jo => jo.Job.JobID == newJob.JobID));

                Assert.IsTrue(s.ApproveOpening(gottenOpeningRequest, approveNotes));
                Assert.AreEqual(1, db.JobOpenings.Count(o => o.Status == OpeningStatus.APPROVED && o.Job.JobID == newJob.JobID && o.StoreManagerNotes == approveNotes));
                Assert.AreEqual(0, db.JobOpenings.Count(o => o.Status == OpeningStatus.PENDING_APPROVAL && o.Job.JobID == newJob.JobID));
                Assert.AreEqual(0, db.JobOpenings.Count(o => o.Status == OpeningStatus.REJECTED && o.Job.JobID == newJob.JobID));

                var pendingOpenings = s.GetPendingOpenings(newStore.ID);
                var allOpenings = s.GetAllOpenings(newStore.ID);
                var approvedOpenings = s.GetApprovedOpenings(newStore.ID);
                var rejectedOpenings = s.GetRejectedOpenings(newStore.ID);

                Assert.AreEqual(0, pendingOpenings.Count());
                Assert.AreEqual(1, allOpenings.Count());
                Assert.AreEqual(1, approvedOpenings.Count());
                Assert.AreEqual(0, rejectedOpenings.Count());

                db.JobOpenings.RemoveRange(db.JobOpenings.Where(o => o.Job.JobID == newJob.JobID));
                db.Jobs.RemoveRange(db.Jobs.Where(j => j.JobID == newJob.JobID));
                db.Stores.RemoveRange(db.Stores.Where(st => st.ID == newStore.ID));
                db.SaveChanges();
            }
        }

        //[TestMethod]
        public void OpeningSvc_Sanity()
        {
            var s = new OpeningSvcClient();
            var excepted = false;
            using (var db = new AESDbContext())
            {
                try
                {
                    var store = db.Stores.FirstOrDefault();

                    var job = db.Jobs.FirstOrDefault();

                    var opening1Table = new JobOpening()
                    {
                        Job = job,
                        Positions = 1,
                        Store = store,
                        Status = OpeningStatus.PENDING_APPROVAL
                    };

                    var opening2Table = new JobOpening()
                    {
                        Job = job,
                        Positions = 1,
                        Store = store,
                        Status = OpeningStatus.PENDING_APPROVAL
                    };

                    var opening3Table = new JobOpening()
                    {
                        Job = job,
                        Positions = 1,
                        Store = store,
                        Status = OpeningStatus.PENDING_APPROVAL
                    };

                    db.JobOpenings.AddOrUpdate(opening1Table, opening2Table, opening3Table);

                    db.SaveChanges();

                    var opening1 = new JobOpeningContract(opening1Table);
                    var opening2 = new JobOpeningContract(opening2Table);
                    var opening3 = new JobOpeningContract(opening3Table);

                    s.RequestOpenings(store.ID, opening1, 1);
                    s.ApproveOpening(opening2, "note");
                    s.GetAllOpenings(store.ID);
                    s.GetApprovedOpenings(store.ID);
                    s.GetPendingOpenings(store.ID);
                    s.GetRejectedOpenings(store.ID);
                    s.RejectOpening(opening3, "note");
                    
                }
                catch (Exception)
                {
                    excepted = true;
                }
            }

            s.Close();
            Assert.IsFalse(excepted);
        }
    }
}
