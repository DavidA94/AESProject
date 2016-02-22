﻿using System;
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
        public ApplicantInfoContract ValidateUser(ApplicantInfoContract userInfo)
        {
            // If we get any null data, just return (DateTime cannot be null)
            if(userInfo.FirstName == null || userInfo.LastName == null || userInfo.SSN == null)
            {
                return null;
            }

            // Get the Context
            using (var db = new ApplicantDbContext())
            {
                // Get the encrypted SSN
                var ssn = Encryption.Encrypt(userInfo.SSN);
                
                // Get the user from the database
                var user = db.ApplicantUsers.FirstOrDefault(u => u.SSN == ssn);

                // If we get a user
                if(user != null)
                {
                    // Ensure the data is correct
                    if (user.FirstName == userInfo.FirstName &&
                        user.LastName == userInfo.LastName &&
                        user.DOB == userInfo.DOB)
                    {
                        // If it is, return the user
                        return new ApplicantInfoContract(user);
                    }
                }
                else
                {
                    // Create the user
                    var newUser = createUser(userInfo, ssn, db);

                    // Assuming we get a user back (no issues creating it)
                    if (newUser != null)
                    {
                        // Double check it's in the DB
                        user = db.ApplicantUsers.FirstOrDefault(u => u.SSN == ssn);

                        // Then return it
                        return new ApplicantInfoContract(user);
                    }
                }
            }

            // If we make it to here, it's a bad user
            return null;
        }

        public ApplicantInfoContract GetUser(ApplicantInfoContract user)
        {
            return null;
        }

        private ApplicantInfoContract createUser(ApplicantInfoContract userInfo, string ssn, ApplicantDbContext db)
        {
            // Create a new user
            var newUser = new ApplicantUser()
            {
                DOB = userInfo.DOB,
                FirstName = userInfo.FirstName,
                LastName = userInfo.LastName,
                SSN = ssn,
                UserInfo = new UserInfo()
            };

            // Add them to the database
            db.ApplicantUsers.Add(newUser);
            try {
                // Try to save the changes
                if(db.SaveChanges() != 0)
                {
                    // If that worked, return the user
                    return new ApplicantInfoContract(db.ApplicantUsers.FirstOrDefault(u => u.SSN == ssn &&
                                                          u.FirstName == userInfo.FirstName &&
                                                          u.LastName == userInfo.LastName &&
                                                          u.DOB == userInfo.DOB));
                }
            }
            catch(Exception)
            {
                // Many things could go wrong, but nothing needs to happen, we will fail out below.
            }

            // If we make it to here, we can't create them
            return null;
        }

        public bool Logout(Contracts.ApplicantInfoContract userInfo)
        {
            throw new NotImplementedException();
        }
    }
}
