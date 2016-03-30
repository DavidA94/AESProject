using AES.Web.OpeningService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AES.Web.Models;
using AES.OpeningsSvc;

namespace AES.Web.Controllers
{
    public class JobOpeningsController : Controller
    {
        // How to set up a cookie 

        // GET: JobOpenings
        public ActionResult Index()
        {
            // Contract being returned
            //IOpeningSvc JobOpenings = new OpeningService.OpeningSvcClient();
            OpeningSvc JobOpenings = new OpeningSvc();

            string value = Request.QueryString["StoreID"];

            var getOpening = JobOpenings.GetOpenings(Convert.ToInt32(value));

            List<JobOpeningsViewModel> OpeningList = new List<JobOpeningsViewModel>();

            foreach (var o in getOpening)
            {
                OpeningList.Add(new JobOpeningsViewModel()
                {
                    Title = o.title, 
                    ShortDesc = o.shortDesc, 
                    ID = o.ID, 
                    LongDesc = o.longDesc
                });
            }

            return View(OpeningList);
        }
    }
}