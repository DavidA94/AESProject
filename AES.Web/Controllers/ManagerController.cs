using AES.Shared;
using AES.Web.Authorization;
using AES.Web.Models;
using AES.Web.OpeningService;
using System.Collections.Generic;
using System.Web.Mvc;

namespace AES.Web.Controllers
{
    //[AESAuthorize(BadRedirectURL = "/EmployeeLogin", Role = EmployeeRole.StoreManager)]
    public class ManagerController : Controller
    {
        /// <summary>
        /// The main requests page (uses partial views)
        /// </summary>
        /// <returns></returns>
        public ActionResult Requests()
        {
            if(EmployeeUserManager.GetUser() == null)
            {
                return RedirectToActionPermanent("Login", "EmployeeLogin");
            }

            return View();
        }

        /// <summary>
        /// Sets the status of a request
        /// </summary>
        /// <param name="requestID">The ID of the request we want to change</param>
        /// <param name="status">The status we want the request to change to</param>
        /// <param name="notes">The notes to go into the response</param>
        /// <returns>Text depending if the request was successful. "success" is returned if it was</returns>
        [HttpPost]
        public ActionResult SetRequest(int requestID, OpeningStatus status, string notes)
        {
            IOpeningSvc openSvc = new OpeningSvcClient();

            if(status == OpeningStatus.APPROVED)
            {
                if(!openSvc.ApproveOpening(new JobOpeningContract() { OpeningID = requestID }, notes))
                {
                    return Content("Unable to change the status of this request.\nPlease contact the system administrator if this error continues.");
                }
            }
            else if(status == OpeningStatus.REJECTED)
            {
                if(!openSvc.RejectOpening(new JobOpeningContract() { OpeningID = requestID }, notes))
                {
                    return Content("Unable to change the status of this request.\nPlease contact the system administrator if this error continues.");
                }
            }
            else
            {
                return Content("Bad Request");
            }

            return Content("success");
        }
        
        #region Partial Pages

        /// <summary>
        /// Gets the list of approved requests
        /// </summary>
        /// <returns></returns>
        public ActionResult ApprovedRequests()
        {
            if (EmployeeUserManager.GetUser() == null)
            {
                return Content("Not logged in");
            }

            IOpeningSvc openSvc = new OpeningSvcClient();

            var openings = openSvc.GetApprovedOpenings(EmployeeUserManager.GetUser().StoreID);

            return PartialView("_RequestList", ConvertContractToModel(openings));
        }

        /// <summary>
        /// Gets the list of denied requests
        /// </summary>
        /// <returns></returns>
        public ActionResult DeniedRequests()
        {
            if (EmployeeUserManager.GetUser() == null)
            {
                return Content("Not logged in");
            }

            IOpeningSvc openSvc = new OpeningSvcClient();

            var openings = openSvc.GetRejectedOpenings(EmployeeUserManager.GetUser().StoreID);

            return PartialView("_RequestList", ConvertContractToModel(openings));
        }

        /// <summary>
        /// Gets the list of pending requests
        /// </summary>
        /// <returns></returns>
        public ActionResult PendingRequests()
        {
            if (EmployeeUserManager.GetUser() == null)
            {
                return Content("Not logged in");
            }

            IOpeningSvc openSvc = new OpeningSvcClient();

            var openings = openSvc.GetPendingOpenings(EmployeeUserManager.GetUser().StoreID);

            return PartialView("_RequestList", ConvertContractToModel(openings));
        }

        #endregion

        #region Helpers

        /// <summary>
        /// Converts a JobOpeningContract to a OpeningRequestModel
        /// </summary>
        /// <param name="openings">An enumerable of JobOpeningContracts</param>
        /// <returns>An enumberable of OpeningRequestModels</returns>
        private IEnumerable<OpeningRequestModel> ConvertContractToModel(IEnumerable<JobOpeningContract> openings)
        {
            List<OpeningRequestModel> retList = new List<OpeningRequestModel>();

            foreach(var opening in openings)
            {
                retList.Add(new OpeningRequestModel()
                {
                    JobShortDescription = opening.ShortDescription,
                    JobTitle = opening.title,
                    NumOpenings = opening.Positions,
                    RequestID = opening.OpeningID,
                    RequestNotes = opening.RequestNotes,
                    StoreManagerNotes = opening.StoreManagerNotes
                });
            }

            return retList;
        }

        #endregion

    }
}