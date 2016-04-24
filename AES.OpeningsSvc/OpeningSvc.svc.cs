using AES.Entities.Contexts;
using AES.Entities.Tables;
using AES.OpeningsSvc.Contracts;
using AES.Shared;
using System.Collections.Generic;
using System.Linq;

namespace AES.OpeningsSvc
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "Service1" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select Service1.svc or Service1.svc.cs at the Solution Explorer and start debugging.
    public class OpeningSvc : IOpeningSvc
    {
        public OpeningSvc()
        {
            DBFileManager.SetDataDirectory();
        }

        public List<JobOpeningContract> GetApprovedOpenings(int StoreID)
        {
            return GetOpenings(StoreID, OpeningStatus.APPROVED);
        }

        public List<JobOpeningContract> GetPendingOpenings(int StoreID)
        {
            return GetOpenings(StoreID, OpeningStatus.PENDING_APPROVAL);
        }

        public List<JobOpeningContract> GetRejectedOpenings(int StoreID)
        {
            return GetOpenings(StoreID, OpeningStatus.REJECTED);
        }

        public bool RequestOpening(int StoreID, JobOpeningContract opening)
        {
            using (var db = new AESDbContext())
            {
                var store = db.Stores.Where(a => a.ID == StoreID).FirstOrDefault();
                var job = db.Jobs.Where(j => j.JobID == opening.JobID).FirstOrDefault();

                if(store == null || job  == null)
                {
                    return false;
                }

                var openingRequest = new JobOpening()
                {
                    Store = store,
                    Job = job,
                    Status = OpeningStatus.PENDING_APPROVAL,
                    RequestNotes = opening.RequestNotes
                };

                db.JobOpenings.Add(openingRequest);
                db.SaveChanges();
            }
            
            return true;
        }

        public bool ApproveOpening(JobOpeningContract opening, string notes)
        {
            using (var db = new AESDbContext())
            {
                var gottenOpening = db.JobOpenings.Where(o => o.ID == opening.OpeningID).FirstOrDefault();
                if (gottenOpening == null || gottenOpening.Status != OpeningStatus.PENDING_APPROVAL)
                {
                    return false;
                }

                gottenOpening.StoreManagerNotes = notes;
                gottenOpening.Status = OpeningStatus.APPROVED;

                try
                {
                    if (db.SaveChanges() < 1)
                    {
                        return false;
                    }
                }
                catch (System.Exception ex)
                {

                    throw ex;
                }

                
            }
            return true;
        }

        public bool RejectOpening(JobOpeningContract opening, string notes)
        {
            using (var db = new AESDbContext())
            {
                var gottenOpening = db.JobOpenings.Where(o => o.ID == opening.OpeningID).FirstOrDefault();
                if (gottenOpening == null || gottenOpening.Status != OpeningStatus.PENDING_APPROVAL)
                {
                    return false;
                }

                gottenOpening.StoreManagerNotes = notes;
                gottenOpening.Status = OpeningStatus.REJECTED;

                if (db.SaveChanges() < 1)
                {
                    return false;
                }
            }
            return true;
        }

        public List<JobOpeningContract> GetAllOpenings(int StoreID)
        {
            List<JobOpeningContract> returnedList = new List<JobOpeningContract>();
            using (var db = new AESDbContext())
            {
                var gottenOpenings = db.JobOpenings.Where(o => o.Store.ID == StoreID).ToList();
                foreach (var opening in gottenOpenings)
                {
                    returnedList.Add(new JobOpeningContract(opening));
                }
            }
            return returnedList;
        }

        private List<JobOpeningContract> GetOpenings(int StoreID, OpeningStatus status)
        {
            List<JobOpeningContract> returnedList = new List<JobOpeningContract>();
            using (var db = new AESDbContext())
            {
                var gottenOpenings = db.JobOpenings.Where(o => o.Store.ID == StoreID && o.Status == status).ToList();
                foreach (var opening in gottenOpenings)
                {
                    returnedList.Add(new JobOpeningContract(opening));
                }
            }
            return returnedList;
        }
    }
}
