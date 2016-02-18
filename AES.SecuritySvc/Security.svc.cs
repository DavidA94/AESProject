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

namespace AES.SecuritySvc
{
    public class Security : ISecurity
    {
        public bool IsValidUser(UserLoginInfo userInfo)
        {
            using (var db = new ApplicantDbContext())
            {
                var crypto = new SimpleCrypto.PBKDF2();
                var ssn = crypto.Compute(userInfo.SSN);

                var user = db.ApplicantUsers.FirstOrDefault(u => u.SSN == ssn);

                if(user != null)
                {
                    if (user.FirstName == userInfo.FirstName &&
                        user.LastName == userInfo.LastName &&
                        user.DOB == userInfo.DOB)
                    {
                        return true;
                    }
                }
                else
                {
                    return createUser(userInfo, ssn, db);
                }
            }

            return false;
        }

        private bool createUser(UserLoginInfo userInfo, string ssn, ApplicantDbContext db)
        {

            var newUser = new ApplicantUser()
            {
                DOB = userInfo.DOB,
                FirstName = userInfo.FirstName,
                LastName = userInfo.LastName,
                SSN = ssn,
                UserInfo = new UserInfo()
            };

            db.ApplicantUsers.Add(newUser);
            return db.SaveChanges() != 0;

        }

        public bool LoginUser(UserLoginInfo userInfo)
        {
            throw new NotImplementedException();
        }

        public bool Logout(UserLoginInfo userInfo)
        {
            throw new NotImplementedException();
        }
    }
}
