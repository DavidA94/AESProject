using AES.Entities.Contexts;
using AES.Entities.Tables;
using AES.Shared;
using AES.Shared.Contracts;
using System;
using System.Data.Entity.Validation;
using System.Diagnostics;
using System.Linq;
using System.Security.Cryptography;

namespace AES.SecuritySvc
{
    public class SecuritySvc : ISecuritySvc
    {
        public SecuritySvc()
        {
            DBFileManager.SetDataDirectory();
        }

        public ApplicantInfoContract ValidateUser(ApplicantInfoContract userInfo)
        {
            // If we get any null data, just return (DateTime cannot be null)
            if (userInfo.FirstName == null || userInfo.LastName == null || userInfo.SSN == null)
            {
                return null;
            }

            // Get the Context
            using (var db = new AESDbContext())
            {
                // Get the encrypted SSN
                var ssn = Encryption.Encrypt(userInfo.SSN);

                // Get the user from the database
                var user = db.ApplicantUsers.FirstOrDefault(u => u.SSN == ssn);

                // If we get a user
                if (user != null)
                {
                    // Ensure the data is correct
                    if (user.FirstName == userInfo.FirstName &&
                        user.LastName == userInfo.LastName &&
                        user.DOB == userInfo.DOB)
                    {
                        // If it is, return the user
                        return makeAppInfoContract(user);
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

                        // Then return the user
                        return makeAppInfoContract(user);
                    }
                }
            }

            // If we make it to here, it's a bad user
            return null;
        }

        public EmployeeUserContract ValidateEmployeeUser(EmployeeCredentialsContract credentials)
        {
            if (string.IsNullOrEmpty(credentials.Password) || credentials.Password.Length < 1 || credentials.Email == null || credentials.Email.Length < 6)
            {
                return null;
            }

            using (var db = new AESDbContext())
            {
                var dbUser = db.EmployeeUsers.FirstOrDefault(u => u.Email.ToLower() == credentials.Email.ToLower());

                if (dbUser != null)
                {
                    var passHash = Encryption.ComputeHash(credentials.Password, new SHA256CryptoServiceProvider(), dbUser.Salt);

                    if (passHash.SequenceEqual(dbUser.PasswordHash))
                    {
                        return MakeEmployeeUserContract(dbUser);
                    }
                }
            }

            return null;
        }

        public bool UpdateUserPassword(EmployeeCredentialsContract credentials, string newPassword)
        {
            var user = ValidateEmployeeUser(credentials);
            if(user == null)
            {
                return false;
            }
            
            using (var db = new AESDbContext())
            {
                var salt = Encryption.GetSalt();
                var passwordHash = Encryption.ComputeHash(newPassword, new SHA256CryptoServiceProvider(), salt);

                var dbUser = db.EmployeeUsers.FirstOrDefault(e => e.Email.ToLower() == user.Email.ToLower());

                if(user == null)
                {
                    return false;
                }

                dbUser.PasswordHash = passwordHash;
                dbUser.Salt = salt;

                try
                {
                    db.SaveChanges();
                    return true;
                }
                catch
                {
                    return false;
                }
            }
        }

        public bool CreateEmployee(EmployeeUserContract employeeInfo, string password)
        {

            if (string.IsNullOrEmpty(password) || password.Length < 1 ||
                string.IsNullOrEmpty(employeeInfo.FirstName) || !employeeInfo.FirstName.All(c => char.IsLetterOrDigit(c)) ||
                string.IsNullOrEmpty(employeeInfo.LastName) || !employeeInfo.LastName.All(c => char.IsLetterOrDigit(c)))
            {
                return false;
            }

            var newEmployeeUser = new EmployeeUser()
            {
                FirstName = employeeInfo.FirstName,
                LastName = employeeInfo.LastName,
                MustResetPassword = true,
                Role = employeeInfo.Role,
                StoreID = employeeInfo.StoreID
            };


            var salt = Encryption.GetSalt();
            var passwordHash = Encryption.ComputeHash(password, new SHA256CryptoServiceProvider(), salt);

            newEmployeeUser.PasswordHash = passwordHash;
            newEmployeeUser.Salt = salt;

            using (var db = new AESDbContext())
            {
                // Check if anybody else else the same name
                var numOthers = db.EmployeeUsers.Count(e => e.FirstName == employeeInfo.FirstName &&
                                                            e.LastName == employeeInfo.LastName);

                if (numOthers == 0)
                {
                    newEmployeeUser.Email = string.Format("{0}.{1}@AES.com", employeeInfo.FirstName, employeeInfo.LastName);
                }
                else
                {
                    newEmployeeUser.Email = string.Format("{0}.{1}{2}@AES.com", employeeInfo.FirstName, employeeInfo.LastName, numOthers + 1);
                }

                db.EmployeeUsers.Add(newEmployeeUser);

                var changes = 0;

                try
                {
                    // Try to save the changes
                    changes = db.SaveChanges();
                }
                catch (DbEntityValidationException e)
                {
                    foreach (var eve in e.EntityValidationErrors)
                    {
                        Debug.WriteLine("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                            eve.Entry.Entity.GetType().Name, eve.Entry.State);
                        foreach (var ve in eve.ValidationErrors)
                        {
                            Debug.WriteLine("- Property: \"{0}\", Error: \"{1}\"",
                                ve.PropertyName, ve.ErrorMessage);
                        }
                    }
                    throw;
                }

                if (changes != 0)
                {
                    return true;
                }
            }

            return false;

        }

        private ApplicantInfoContract createUser(ApplicantInfoContract userInfo, string ssn, AESDbContext db)
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
            try
            {
                // Try to save the changes
                if (db.SaveChanges() != 0)
                {
                    // If that worked, return the user
                    return makeAppInfoContract(db.ApplicantUsers.FirstOrDefault(u => u.SSN == ssn &&
                                                                                     u.FirstName == userInfo.FirstName &&
                                                                                     u.LastName == userInfo.LastName &&
                    /**/                                                             u.DOB == userInfo.DOB));
                }
            }
            catch (Exception)
            {
                // Many things could go wrong, but nothing needs to happen, we will fail out below.
            }

            // If we make it to here, we can't create them
            return null;
        }

        private ApplicantInfoContract makeAppInfoContract(ApplicantUser user)
        {
            return new ApplicantInfoContract()
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                UserID = user.userID,
                DOB = user.DOB
            };
        }

        private EmployeeUserContract MakeEmployeeUserContract(EmployeeUser user)
        {

            return new EmployeeUserContract()
            {
                Email = user.Email,
                Role = user.Role,
                FirstName = user.FirstName,
                LastName = user.LastName,
                MustResetPassword = user.MustResetPassword,
                StoreID = user.StoreID,
                StoreName = user.Store.Name
            };
        }
    }
}
