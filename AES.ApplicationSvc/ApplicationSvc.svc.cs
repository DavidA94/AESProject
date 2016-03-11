using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using AES.ApplicationSvc.Contracts;
using AES.Shared;
using AES.Shared.Contracts;
using AES.Entities.Tables;

namespace AES.ApplicationSvc
{
    public class ApplicationSvc : IApplicationSvc
    {
        public bool CancelApplication(ApplicationInfoContract app)
        {
            throw new NotImplementedException();
        }

        public UserInfoContract GetApplicantsAwaitingCalls()
        {
            throw new NotImplementedException();
        }

        public UserInfoContract GetApplicantsAwaitingInterview(int storeID)
        {
            throw new NotImplementedException();
        }

        public ApplicationInfoContract GetApplication(UserInfoContract user)
        {
            throw new NotImplementedException();
        }

        public ApplicationInfoContract GetCallApplication(UserInfoContract user)
        {
            throw new NotImplementedException();
        }

        public ApplicationInfoContract GetInterviewApplication(UserInfoContract user)
        {
            throw new NotImplementedException();
        }

        public bool PullApplicantFromCallQueue(UserInfoContract user)
        {
            throw new NotImplementedException();
        }

        public bool SavePartialApplication(ApplicationInfoContract app)
        {
            var applicant = new Application()
            {
                 Applicant = new ApplicantUser()
                 {
                     Availability = new Availability()
                     {
                         SundayStart = app.Applicant.Availability.
                     }
                 }
            }

            return false;
        }

        public bool SetApplicationStatus(ApplicationInfoContract app, AppStatus status)
        {
            throw new NotImplementedException();
        }

        public bool SubmitApplication(ApplicationInfoContract app)
        {
            throw new NotImplementedException();
        }

        #region Converters

        #region Availability

        private Availability ConvertContractToTable(AvailabilityContract availability)
        {
            // We only care about the time. Using this method where we just get the time will make the date irrelvant
            return new Availability()
            {
                SundayStart = availability.SundayStart,
                SundayEnd = availability.SundayEnd,
                MondayStart = availability.MondayStart,
                MondayEnd = availability.MondayEnd,
                TuesdayStart = availability.TuesdayStart,
                TuesdayEnd = availability.TuesdayEnd,
                WednesdayStart = availability.WednesdayStart,
                WednesdayEnd = availability.WednesdayEnd,
                ThursdayStart = availability.ThursdayStart,
                ThursdayEnd = availability.ThursdayEnd,
                FridayStart = availability.FridayStart,
                FridayEnd = availability.FridayEnd,
                SaturdayStart = availability.SaturdayStart,
                SaturdayEnd = availability.SaturdayEnd
            };
        }

        private AvailabilityContract ConvertTableToContract(Availability availability)
        {
            return new AvailabilityContract()
            {
                SundayStart = availability.SundayStart,
                SundayEnd = availability.SundayEnd,
                MondayStart = availability.MondayStart,
                MondayEnd = availability.MondayEnd,
                TuesdayStart = availability.TuesdayStart,
                TuesdayEnd = availability.TuesdayEnd,
                WednesdayStart = availability.WednesdayStart,
                WednesdayEnd = availability.WednesdayEnd,
                ThursdayStart = availability.ThursdayStart,
                ThursdayEnd = availability.ThursdayEnd,
                FridayStart = availability.FridayStart,
                FridayEnd = availability.FridayEnd,
                SaturdayStart = availability.SaturdayStart,
                SaturdayEnd = availability.SaturdayEnd
            ;
        }

        #endregion

        #region ApplicantUser

        private ApplicantUser ConvertContractToTable(ApplicantInfoContract applicant)
        {
            return new ApplicantUser()
            {
                Availability = ConvertContractToTable(applicant.Availability),
                CallEndTime = applicant.EndCallTime,
                CallStartTime = applicant.StartCallTime,
                DOB = applicant.DOB,
                FirstName = applicant.FirstName,
                LastName = applicant.LastName,
                UserInfo = ConvertContractToTable(applicant.UserInfo),
            };
        }

        #endregion

        #region UserInfo

        private UserInfo ConvertContractToTable(UserInfoContract info)
        {
            return new UserInfo()
            {
                Address = info.Address,
                City = info.City,
                Nickname = info.Nickname,
                Phone = info.Phone,
                SalaryExpectation = info.SalaryExpectation,
                State = info.State,
                Zip = info.Zip
            };
        }

        #endregion

        #endregion
    }
}
