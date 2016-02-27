﻿using AES.Entities.Contexts;
using AES.OpeningSvc.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

namespace AES.OpeningsSvc
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "Service1" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select Service1.svc or Service1.svc.cs at the Solution Explorer and start debugging.
    public class OpeningSvc : IOpeningSvc
    {
        public List<JobOpeningContract> GetOpenings(int StoreID)
        {
            List<JobOpeningContract> returnedList = new List<JobOpeningContract>();
            using (var db = new OpeningDbContext())
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
