using FastAndEasy.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace FastAndEasy.Controllers
{
    [Authorize]
    public class AdminController : Controller
    {
        FastAndEasyEntities db = new FastAndEasyEntities();
        // GET: Admin
        public ActionResult Index()
        {
            return View();
        }

        public JsonResult getManager()
        {
            if (Request.IsAjaxRequest())
            {
                FastAndEasyEntities db = new FastAndEasyEntities();
                var manager = db.Managers.Select(m => new { m.Id, m.UserName, m.Email, m.Phone, m.LastName, m.FirstName, m.ImagePath }).FirstOrDefault();
                return new JsonResult { Data = manager, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            }

            return new JsonResult { Data = "error", JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        
        public JsonResult editManager(string LastName, string FirstName, string UserName, string pass, string Phone, string Email)
        {
            string UserNameError = " ";
            string LNameError = " ";
            string FNameError = " ";
            string PassError = " ";
            string PhoneError = " ";
            string EmailError = " ";
            var rName = new Regex(@"^[A-Z][a-z]+$");
            var hasNumber = new Regex(@"[0-9]+");
            var hasChar = new Regex(@"[A-Za-z]+");
            var hasMiniMaxChars = new Regex(@".{6, }");            
            var rLogin = new Regex(@"^[A-Za-z\d]+$");
            var rPhone = new Regex(@"^\+9989[0-9]{8}$");
            var rEmail = new Regex(@"^.+$");
            FastAndEasyEntities db = new FastAndEasyEntities();
            if (!rName.IsMatch(LastName))
            {
                LNameError = "Lastname should only contain letters with a capital letter at the beginning and not be empty! Example: 'Khamzaev'!";                
            }
            if (!rName.IsMatch(FirstName))
            {
                FNameError = "Firstname should only contain letters with a capital letter at the beginning and not be empty! Example: 'Asilbek'!";                
            }
            if (!rLogin.IsMatch(UserName))
            {
                UserNameError = "Username should only contain letters and numbers and not be empty! Example: 'AceTheKing', 'ACETHEKING', 'acetheking' or 'acetheking7776'!";                
            }
            //if (ifAExists(UserName) != null)
            //{
            //    UserNameError = "The user already exists!";                
            //}
            if (ifMExists(UserName) != null)
            {
                UserNameError = "The user already exists!";                
            }
            if (ifRExists(UserName) != null)
            {
                UserNameError = "The user already exists!";               
            }
            if (ifTExists(UserName) != null)
            {
                UserNameError = "The user already exists!";                
            }
            if (ifSExists(UserName) != null)
            {
                UserNameError = "The user already exists!";                
            }
            if (!hasNumber.IsMatch(pass))
            {
                PassError = "The password should contain at least one number!";            
            }
            else if (!hasChar.IsMatch(pass))
            {
                PassError = "The password should contain at least one letter!";               
            }
            else if (!hasMiniMaxChars.IsMatch(pass))
            {
                PassError = "The password should be at least six characters long!";               
            }
            else
            {
                PassError = " ";
            }
            if (!rPhone.IsMatch(Phone))
            {
                PhoneError = "Enter full UZB phone number! Example: '+998945677776'";               
            }
            if(!string.IsNullOrEmpty(Email) && !string.IsNullOrWhiteSpace(Email))
            {
                if (!IsValidEmail(Email))
                {
                    EmailError = "Invalid email address!";
                }
            }                       
            if (UserNameError == " " && PassError == " " && LNameError == " " && FNameError == " "
                && PhoneError == " " && EmailError == " ")
            {
                Manager manager = new Manager();
                manager = db.Managers.Find(1);
                manager.UserName = UserName;
                manager.Password = Crypto.HashPassword(pass);
                manager.LastName = LastName;
                manager.FirstName = FirstName;
                manager.Phone = Phone;
                manager.Email = Email;
                manager.RoleId = 2;
                db.Entry(manager).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
                ModelState.Clear();
                 var result = "success";
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            else {
                var result = new { LNameError, FNameError, UserNameError, PassError, PhoneError, EmailError };
                return Json(result, JsonRequestBehavior.AllowGet);
            }          

        }



        public ActionResult IndexReception()
        {
            return View();
        }

        public JsonResult getReception()
        {
            if (Request.IsAjaxRequest())
            {
                var reception = db.Receptions.OrderBy(r => r.LastName).Select(m=> new { m.Id, m.UserName, m.Email, m.Phone, m.LastName, m.FirstName, m.ImagePath }).ToList();
                return new JsonResult { Data = reception, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            }
            return new JsonResult { Data = "Error", JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        public JsonResult getReceptionist(string id)
        {
            int Id = int.Parse(id);
            if (Request.IsAjaxRequest())
            {
                var receptionist = db.Receptions.Where(r=> r.Id == Id).Select(m => new { m.Id, m.UserName, m.Email, m.Phone, m.LastName, m.FirstName, m.ImagePath }).FirstOrDefault();
                return new JsonResult { Data = receptionist, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            }
            return new JsonResult { Data = "Error", JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        public JsonResult updateReceptionist(string id, string LastName, string FirstName, string UserName, string pass, string Phone, string Email)
        {
            string UserNameError = " ";
            string LNameError = " ";
            string FNameError = " ";
            string PassError = " ";
            string PhoneError = " ";
            string EmailError = " ";
            var rName = new Regex(@"^[A-Z][a-z]+$");
            var hasNumber = new Regex(@"[0-9]+");
            var hasChar = new Regex(@"[A-Za-z]+");
            var hasMiniMaxChars = new Regex(@".{6, }");
            var rLogin = new Regex(@"^[A-Za-z\d]+$");
            var rPhone = new Regex(@"^\+9989[0-9]{8}$");
            var rEmail = new Regex(@"^.+$");
            FastAndEasyEntities db = new FastAndEasyEntities();
            int Id = int.Parse(id);
            if (!rName.IsMatch(LastName))
            {
                LNameError = "Lastname should only contain letters with a capital letter at the beginning and not be empty! Example: 'Khamzaev'!";
            }
            if (!rName.IsMatch(FirstName))
            {
                FNameError = "Firstname should only contain letters with a capital letter at the beginning and not be empty! Example: 'Asilbek'!";
            }
            if (!rLogin.IsMatch(UserName))
            {
                UserNameError = "Username should only contain letters and numbers and not be empty! Example: 'AceTheKing', 'ACETHEKING', 'acetheking' or 'acetheking7776'!";
            }
            //if (ifAExists(UserName) != null)
            //{
            //    UserNameError = "The user already exists!";                
            //}
            if (ifMExists(UserName) != null)
            {
                UserNameError = "The user already exists!";
            }
            if (ifRExists(UserName) != null)
            {
                UserNameError = "The user already exists!";
            }
            if (ifTExists(UserName) != null)
            {
                UserNameError = "The user already exists!";
            }
            if (ifSExists(UserName) != null)
            {
                UserNameError = "The user already exists!";
            }
            if (!hasNumber.IsMatch(pass))
            {
                PassError = "The password should contain at least one number!";
            }
            else if (!hasChar.IsMatch(pass))
            {
                PassError = "The password should contain at least one letter!";
            }
            else if (!hasMiniMaxChars.IsMatch(pass))
            {
                PassError = "The password should be at least six characters long!";
            }
            else
            {
                PassError = " ";
            }
            if (!rPhone.IsMatch(Phone))
            {
                PhoneError = "Enter full UZB phone number! Example: '+998945677776'";
            }
            if (!string.IsNullOrEmpty(Email) && !string.IsNullOrWhiteSpace(Email))
            {
                if (!IsValidEmail(Email))
                {
                    EmailError = "Invalid email address!";
                }
            }
            if (UserNameError == " " && PassError == " " && LNameError == " " && FNameError == " "
                && PhoneError == " " && EmailError == " ")
            {
                Reception manager = new Reception();
                manager = db.Receptions.Find(Id);
                manager.UserName = UserName;
                manager.Password = Crypto.HashPassword(pass);
                manager.LastName = LastName;
                manager.FirstName = FirstName;
                manager.Phone = Phone;
                manager.Email = Email;
                manager.RoleId = 3;
                db.Entry(manager).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
                ModelState.Clear();
                return Json("", JsonRequestBehavior.AllowGet);
            }
            else
            {
                var result = new { LNameError, FNameError, UserNameError, PassError, PhoneError, EmailError };
                return Json(result, JsonRequestBehavior.AllowGet);
            }       
           
            
        }

        public JsonResult addNewReceptionist( string LastName, string FirstName, string UserName, string pass, string Phone, string Email)
        {
            
            FastAndEasyEntities db = new FastAndEasyEntities();
            Reception manager = new Reception();            
            manager.UserName = UserName;
            manager.Password = Crypto.HashPassword(pass);
            manager.LastName = LastName;
            manager.FirstName = FirstName;
            manager.Phone = Phone;
            manager.Email = Email;
            manager.RoleId = 3;
            db.Receptions.Add(manager);
            db.SaveChanges();
            ModelState.Clear();
            return Json("", JsonRequestBehavior.AllowGet);
        }

        public ActionResult IndexTeacher()
        {
            return View();
        }

        public JsonResult getTeachers()
        {
            if (Request.IsAjaxRequest())
            {
                var reception = db.Teachers.OrderBy(r => r.LastName).Select(m => new { m.Id, m.UserName, m.Email, m.Phone, m.LastName, m.FirstName, m.ImagePath }).ToList();
                return new JsonResult { Data = reception, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            }
            return new JsonResult { Data = "Error", JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        public JsonResult getTeacher(string id)
        {
            int Id = int.Parse(id);
            if (Request.IsAjaxRequest())
            {
                var receptionist = db.Teachers.Where(r => r.Id == Id).Select(m => new { m.Id, m.UserName, m.Email, m.Phone, m.LastName, m.FirstName, m.ImagePath }).FirstOrDefault();
                return new JsonResult { Data = receptionist, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            }
            return new JsonResult { Data = "Error", JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        public JsonResult updateTeacher(string id, string LastName, string FirstName, string UserName, string pass, string Phone, string Email)
        {
            int Id = int.Parse(id);
            FastAndEasyEntities db = new FastAndEasyEntities();
            Teacher manager = new Teacher();
            manager = db.Teachers.Find(Id);
            manager.UserName = UserName;
            manager.Password = Crypto.HashPassword(pass);
            manager.LastName = LastName;
            manager.FirstName = FirstName;
            manager.Phone = Phone;
            manager.Email = Email;
            manager.RoleId = 4;
            db.Entry(manager).State = System.Data.Entity.EntityState.Modified;
            db.SaveChanges();
            ModelState.Clear();
            return Json("", JsonRequestBehavior.AllowGet);
        }

        public bool IsValidEmail(string emailaddress)
        {
            try
            {
                MailAddress m = new MailAddress(emailaddress);

                return true;
            }
            catch (FormatException)
            {
                return false;
            }
        }

        private Admin ifAExists(string UserName)
        {
            return db.Admins.FirstOrDefault(a => a.UserName == UserName);
        }
        private Manager ifMExists(string UserName)
        {
            return db.Managers.FirstOrDefault(a => a.UserName == UserName);
        }

        private Reception ifRExists(string UserName)
        {
            return db.Receptions.FirstOrDefault(a => a.UserName == UserName);
        }

        private Teacher ifTExists(string UserName)
        {
            return db.Teachers.FirstOrDefault(a => a.UserName == UserName);
        }

        private Student ifSExists(string UserName)
        {
            return db.Students.FirstOrDefault(a => a.UserName == UserName);
        }

    }
}