using System.Collections.Generic;


namespace AES.Entities.Tables
{
    public class SubmittedApplications
    {
        public virtual ICollection<Application> Applications { get; set; }
    }
}
