using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using AES.SecuritySvc.Contracts;
using AES.Entities.Contexts;
using AES.Entities.Tables;
using System.Data.Entity;

namespace AES.SecuritySvc
{
    public class Security : ISecurity
    {
        static public readonly string SSNSALT = "1.OMICRON!";

        public UserInfoContract ValidateUser(UserInfoContract userInfo)
        {
            using (var db = new ApplicantDbContext())
            {
                var crypto = new SimpleCrypto.PBKDF2();
                var ssn = crypto.Compute(userInfo.SSN, SSNSALT);

                var user = db.ApplicantUsers.FirstOrDefault(u => u.SSN == ssn);

                if(user != null)
                {
                    if (user.FirstName == userInfo.FirstName &&
                        user.LastName == userInfo.LastName &&
                        user.DOB == userInfo.DOB)
                    {
                        return new UserInfoContract(user);
                    }
                }
                else
                {
                    var newUser = createUser(userInfo, ssn, db);
                    if (newUser != null)
                    {
                        // Although we have it, double check it's in the DB
                        user = db.ApplicantUsers.FirstOrDefault(u => u.SSN == ssn);
                        return new UserInfoContract(user);
                    }
                }
            }

            return null;
        }

        private UserInfoContract createUser(UserInfoContract userInfo, string ssn, ApplicantDbContext db)
        {
            var newUser = new ApplicantUser()
            {
                DOB = userInfo.DOB,
                FirstName = userInfo.FirstName,
                LastName = userInfo.LastName,
                SSN = ssn,
                UserInfo = new Entities.Tables.UserInfo()
            };

            db.ApplicantUsers.Add(newUser);
            try {
                if(db.SaveChanges() != 0)
                {
                    return new UserInfoContract(db.ApplicantUsers.FirstOrDefault(u => u.SSN == ssn &&
                                                          u.FirstName == userInfo.FirstName &&
                                                          u.LastName == userInfo.LastName &&
                                                          u.DOB == userInfo.DOB));
                }
            }
            catch(Exception)
            {
                return null;
            }

            return null;
        }

        public bool Logout(Contracts.UserInfoContract userInfo)
        {
            throw new NotImplementedException();
        }
    }
}
