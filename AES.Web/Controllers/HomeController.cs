using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AES.Web.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult HomePage()
        {
            if(HttpContext.Request.QueryString["store"] == null)
            {
                return RedirectPermanent(Request.Url.AbsolutePath + "?store=1");
            }

            return View();
        }
    }
}