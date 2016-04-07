using AES.Entities.Contexts;
using AES.OpeningsSvc.Contracts;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace AES.OpeningsSvc
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "Service1" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select Service1.svc or Service1.svc.cs at the Solution Explorer and start debugging.
    public class OpeningSvc : IOpeningSvc
    {
        public OpeningSvc()
        {
            string data = AppDomain.CurrentDomain.GetData("DataDirectory").ToString();
            if (data.Contains("bin") || data.ToLower().Contains("app_data"))
            {
                // Get the directory we're starting in
                DirectoryInfo dir = new DirectoryInfo(AppDomain.CurrentDomain.BaseDirectory);

                // Loop until we find the folder that holds AES.Web
                while (dir.GetDirectories().FirstOrDefault(d => d.Name == "AES.Web") == null)
                {
                    dir = dir.Parent;
                }

                // Set the DataDirectory
                AppDomain.CurrentDomain.SetData("DataDirectory", dir.FullName);
            }
        }

        public List<JobOpeningContract> GetOpenings(int StoreID)
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
    }
}
