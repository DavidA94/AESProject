using System.Collections.Generic;


namespace AES.Entities.Tables
{
    public class SubmittedApplications
    {

        public  SubmittedApplications()
        {
            Applications = new HashSet<Application>();
        }

        public virtual ICollection<Application> Applications { get; set; }
    }
}
