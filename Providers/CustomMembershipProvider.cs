using FastAndEasy.Models;
using System;
using System.Configuration;
using System.Linq;
using System.Web.Helpers;
using System.Web.Security;


namespace FastAndEasy.Providers
{
    public class CustomMembershipProvider : MembershipProvider
    {
        public override bool ValidateUser(string username, string password)
        {
            bool isValid = false;

            using (FastAndEasyEntities _db = new FastAndEasyEntities())
            {
                try
                {
                    Admin admin = (from a in _db.Admins
                                 where a.Email == username || a.UserName == username
                                 select a).FirstOrDefault();
                    SettingsContext context = new SettingsContext();
                    string adminName = (string)context["UserName"];
                    int aID = _db.Admins.Where(a => a.Email.Equals(adminName) || a.UserName.Equals(adminName)).FirstOrDefault().Id;

                    if (admin != null && Crypto.VerifyHashedPassword(admin.Password, password))
                    {
                        isValid = true;
                    }

                    Manager manager = (from m in _db.Managers
                                         where m.Email == username || m.UserName == username
                                         select m).FirstOrDefault();
                    if (manager != null && Crypto.VerifyHashedPassword(manager.Password, password))
                    {
                        isValid = true;
                    }

                    Reception receptionist = (from r in _db.Receptions
                                     where r.Email == username || r.UserName == username
                                     select r).FirstOrDefault();
                    if (receptionist != null && Crypto.VerifyHashedPassword(receptionist.Password, password))
                    {
                        isValid = true;
                    }
                    Teacher teacher = (from t in _db.Teachers
                                   where t.Email == username || t.UserName == username
                                       select t).FirstOrDefault();

                    if (teacher != null && Crypto.VerifyHashedPassword(teacher.Password, password))
                    {
                        isValid = true;
                    }

                    Student student = (from s in _db.Students
                                   where  s.UserName == username || s.Email == username
                                   select s).FirstOrDefault();

                    string pupName = (string)context["UserName"];
                    int pID = _db.Students.Where(p => p.UserName.Equals(pupName)).FirstOrDefault().Id;

                    if (student != null && Crypto.VerifyHashedPassword(student.Password, password))
                    {
                        isValid = true;
                    }

                }
                catch
                {
                    isValid = false;
                }
            }
            return isValid;
        }

        public MembershipUser CreateUser(string email, string password)
        {
            MembershipUser membershipUser = GetUser(email, false);

            if (membershipUser == null)
            {
                try
                {
                    using (FastAndEasyEntities _db = new FastAndEasyEntities())
                    {
                        Admin admin = new Admin();
                        admin.Email = email;
                        admin.Password = Crypto.HashPassword(password);
                        //admin.CreationDate = DateTime.Now;

                        if (_db.Roles.Find(1) != null)
                        {
                            admin.RoleId = 1;
                        }

                        

                        _db.Admins.Add(admin);
                        _db.SaveChanges();
                        membershipUser = GetUser(email, false);
                        return membershipUser;

#pragma warning disable CS0162 // Unreachable code detected
                        Manager director = new Manager();
#pragma warning restore CS0162 // Unreachable code detected
                        director.Email = email;
                        director.Password = Crypto.HashPassword(password);
                        //admin.CreationDate = DateTime.Now;

                        if (_db.Roles.Find(2) != null)
                        {
                            director.RoleId = 2;
                        }

                        _db.Managers.Add(director);
                        _db.SaveChanges();
                        membershipUser = GetUser(email, false);
                        return membershipUser;



#pragma warning disable CS0162 // Unreachable code detected
                        Reception parent = new Reception();
#pragma warning restore CS0162 // Unreachable code detected
                        parent.Email = email;
                        parent.Password = Crypto.HashPassword(password);

                        if(_db.Roles.Find(3) != null)
                        {
                            parent.RoleId = 3;
                        }

                        _db.Receptions.Add(parent);
                        _db.SaveChanges();
                        membershipUser = GetUser(email, false);
                        return membershipUser;

#pragma warning disable CS0162 // Unreachable code detected
                        Teacher teacher = new Teacher();
#pragma warning restore CS0162 // Unreachable code detected
                        teacher.Email = email;
                        teacher.Password = Crypto.HashPassword(password);
                        //admin.CreationDate = DateTime.Now;

                        if (_db.Roles.Find(4) != null)
                        {
                            teacher.RoleId = 4;
                        }

                        _db.Teachers.Add(teacher);
                        _db.SaveChanges();
                        membershipUser = GetUser(email, false);
                        return membershipUser;

                        Student pupil = new Student();
                        pupil.Email = email;
                        pupil.Password = Crypto.HashPassword(password);
                        //admin.CreationDate = DateTime.Now;

                        if (_db.Roles.Find(5) != null)
                        {
                            pupil.RoleId = 5;
                        }

                        _db.Students.Add(pupil);
                        _db.SaveChanges();
                        membershipUser = GetUser(email, false);
                        return membershipUser;
                    }
                }
                catch
                {
                    return null;
                }
            }
            return null;
        }

        //public override MembershipUser GetUser(string email, bool userIsOnline)
        //{
        //    try
        //    {
        //        using (UserContext _db = new UserContext())
        //        {
        //            var users = from u in _db.Users
        //                        where u.Email == email
        //                        select u;
        //            if (users.Count() > 0)
        //            {
        //                User user = users.First();
        //                MembershipUser memberUser = new MembershipUser("MyMembershipProvider", user.Email, null, null, null, null,
        //                    false, false, user.CreationDate, DateTime.MinValue, DateTime.MinValue, DateTime.MinValue, DateTime.MinValue);
        //                return memberUser;
        //            }
        //        }
        //    }
        //    catch
        //    {
        //        return null;
        //    }
        //    return null;
        //}

        public override string ApplicationName
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public override bool ChangePassword(string username, string oldPassword, string newPassword)
        {
            throw new NotImplementedException();
        }

        public override bool ChangePasswordQuestionAndAnswer(string username, string password, string newPasswordQuestion, string newPasswordAnswer)
        {
            throw new NotImplementedException();
        }

        public override MembershipUser CreateUser(string username, string password, string email, string passwordQuestion, string passwordAnswer, bool isApproved, object providerUserKey, out MembershipCreateStatus status)
        {
            throw new NotImplementedException();
        }

        public override bool DeleteUser(string username, bool deleteAllRelatedData)
        {
            throw new NotImplementedException();
        }

        public override bool EnablePasswordReset
        {
            get { throw new NotImplementedException(); }
        }
        public override bool EnablePasswordRetrieval
        {
            get { throw new NotImplementedException(); }
        }
        public override MembershipUserCollection FindUsersByEmail(string emailToMatch, int pageIndex, int pageSize, out int totalRecords)
        {
            throw new NotImplementedException();
        }
        public override MembershipUserCollection FindUsersByName(string usernameToMatch, int pageIndex, int pageSize, out int totalRecords)
        {
            throw new NotImplementedException();
        }
        public override MembershipUserCollection GetAllUsers(int pageIndex, int pageSize, out int totalRecords)
        {
            throw new NotImplementedException();
        }
        public override int GetNumberOfUsersOnline()
        {
            throw new NotImplementedException();
        }
        public override string GetPassword(string username, string answer)
        {
            throw new NotImplementedException();
        }
        public override MembershipUser GetUser(object providerUserKey, bool userIsOnline)
        {
            throw new NotImplementedException();
        }
        public override string GetUserNameByEmail(string email)
        {
            throw new NotImplementedException();
        }
        public override int MaxInvalidPasswordAttempts
        {
            get { throw new NotImplementedException(); }
        }
        public override int MinRequiredNonAlphanumericCharacters
        {
            get { throw new NotImplementedException(); }
        }
        public override int MinRequiredPasswordLength
        {
            get { throw new NotImplementedException(); }
        }
        public override int PasswordAttemptWindow
        {
            get { throw new NotImplementedException(); }
        }
        public override MembershipPasswordFormat PasswordFormat
        {
            get { throw new NotImplementedException(); }
        }
        public override string PasswordStrengthRegularExpression
        {
            get { throw new NotImplementedException(); }
        }
        public override bool RequiresQuestionAndAnswer
        {
            get { throw new NotImplementedException(); }
        }
        public override bool RequiresUniqueEmail
        {
            get { throw new NotImplementedException(); }
        }
        public override string ResetPassword(string username, string answer)
        {
            throw new NotImplementedException();
        }
        public override bool UnlockUser(string userName)
        {
            throw new NotImplementedException();
        }
        public override void UpdateUser(MembershipUser user)
        {
            throw new NotImplementedException();
        }

        public override MembershipUser GetUser(string username, bool userIsOnline)
        {
            throw new NotImplementedException();
        }
    }
}