using FastAndEasy.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;

namespace FastAndEasy.Controllers
{
    [Authorize]
    public class StudentController : Controller
    {
        // GET: Student
        public ActionResult Index(int id)
        {
            FastAndEasyEntities _db = new FastAndEasyEntities();
            string receptionistName = HttpContext.User.Identity.Name;
            int rId = _db.Students.Where(p => (p.Email.Equals(receptionistName)) || (p.UserName.Equals(receptionistName))).FirstOrDefault().Id;
            if (id == rId)
            {
                ViewBag.Id = rId;
                Student d = _db.Students.Find(id);
                return View(d);
            }
            return View();
        }
        [Authorize]
        public ActionResult Edit(int id)
        {
            FastAndEasyEntities _db = new FastAndEasyEntities();
            string receptionistName = HttpContext.User.Identity.Name;
            int rId = _db.Students.Where(p => (p.Email.Equals(receptionistName)) || (p.UserName.Equals(receptionistName))).FirstOrDefault().Id;
            if (id == rId)
            {
                Student d = _db.Students.Find(id);
                return View(d);
            }
            return View();
        }

        [HttpPost]
        public ActionResult Edit(HttpPostedFileBase ImageFile, Student d, string pass, string passConf)
        {
            if (ModelState.IsValid)
            {
                if (pass == passConf)
                {
                    var db = new FastAndEasyEntities();
                    db.Students.Attach(d);
                    if (ImageFile != null)
                    {
                        string fileName = Path.GetFileNameWithoutExtension(ImageFile.FileName);
                        string fileExtension = Path.GetExtension(ImageFile.FileName);
                        fileName = fileName + DateTime.Now.ToString("yymmssfff") + fileExtension;
                        d.ImagePath = "/img/photos/reception/" + fileName;
                        fileName = Path.Combine(Server.MapPath("/img/photos/reception/"), fileName);
                        ImageFile.SaveAs(fileName);
                    }


                    db.Entry(d).Property(p => p.LastName).IsModified = true;
                    db.Entry(d).Property(p => p.FirstName).IsModified = true;
                    db.Entry(d).Property(p => p.DoB).IsModified = true;
                    db.Entry(d).Property(p => p.Address).IsModified = true;
                    db.Entry(d).Property(p => p.Phone).IsModified = true;
                    db.Entry(d).Property(p => p.Email).IsModified = true;
                    db.Entry(d).Property(p => p.ImagePath).IsModified = true;
                    db.SaveChanges();

                    return RedirectToAction("Index", new { id = d.Id });
                }
                else
                {
                    ViewBag.Pass = "Пароли не совпадают";
                    return View();
                }
            }
            return View();

        }

        public ActionResult EditPassword(int id)
        {
            FastAndEasyEntities _db = new FastAndEasyEntities();
            string receptionistName = HttpContext.User.Identity.Name;
            int rId = _db.Students.Where(p => (p.Email.Equals(receptionistName)) || (p.UserName.Equals(receptionistName))).FirstOrDefault().Id;
            if (id == rId)
            {
                Student d = _db.Students.Find(id);
                return View(d);
            }
            return View();
        }
        [HttpPost]
        public ActionResult EditPassword(string oldPass, string newPass, string newPassConfirm, Student d)
        {
            FastAndEasyEntities db = new FastAndEasyEntities();
            string receptionistName = HttpContext.User.Identity.Name;
            string pass = db.Students.Where(p => (p.Email.Equals(receptionistName)) || (p.UserName.Equals(receptionistName))).FirstOrDefault().Password;
            ViewBag.Pass = pass;

            if (Crypto.VerifyHashedPassword(pass, oldPass))
            {
                if ((newPass == newPassConfirm))
                {
                    db.Students.Attach(d);
                    d.Password = Crypto.HashPassword(newPass);
                    db.Entry(d).Property(p => p.Password).IsModified = true;
                    db.SaveChanges();
                    return RedirectToAction("Index", new { id = d.Id });
                }
                else
                {
                    ViewBag.Error = "Passwords Do not Match";
                }
            }
            else
            {
                ViewBag.Error = "Wrong Current Password";
            }

            return View();
        }
    }
}