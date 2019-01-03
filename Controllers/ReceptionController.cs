using FastAndEasy.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;

namespace FastAndEasy.Controllers
{
    [Authorize]
    public class ReceptionController : Controller
    {
        FastAndEasyEntities db = new FastAndEasyEntities();
        // GET: Reception
        public ActionResult Index(int id)
        {
            FastAndEasyEntities db = new FastAndEasyEntities();
            int x = db.Receptions.Where(t => t.Id == id).FirstOrDefault().Id;
            ViewBag.Id = x;
            return View(x);
        }

        public ActionResult Edit(int id)
        {
            FastAndEasyEntities _db = new FastAndEasyEntities();
            string receptionistName = HttpContext.User.Identity.Name;
            int rId = _db.Receptions.Where(p => (p.Email.Equals(receptionistName)) || (p.UserName.Equals(receptionistName))).FirstOrDefault().Id;
            if (id == rId)
            {
                Reception d = _db.Receptions.Find(id);
                return View(d);
            }
            return View();
        }
        [HttpPost]
        public ActionResult Edit(HttpPostedFileBase ImageFile, Reception d, string pass, string passConf)
        {
            if (ModelState.IsValid)
            {
                if (pass == passConf)
                {
                    var db = new FastAndEasyEntities();
                    db.Receptions.Attach(d);
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
            int rId = _db.Receptions.Where(p => (p.Email.Equals(receptionistName)) || (p.UserName.Equals(receptionistName))).FirstOrDefault().Id;
            if (id == rId)
            {
                Reception d = _db.Receptions.Find(id);
                return View(d);
            }
            return View();
        }
        [HttpPost]
        public ActionResult EditPassword(string oldPass, string newPass, string newPassConfirm, Reception d)
        {
            FastAndEasyEntities _db = new FastAndEasyEntities();
            string receptionistName = HttpContext.User.Identity.Name;
            string pass = _db.Receptions.Where(p => (p.Email.Equals(receptionistName)) || (p.UserName.Equals(receptionistName))).FirstOrDefault().Password;
            ViewBag.Pass = pass;
            
            if (Crypto.VerifyHashedPassword(pass, oldPass))
            {
                if((newPass == newPassConfirm))
                {
                    db.Receptions.Attach(d);
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

        public JsonResult getTeachers()
        {
            if (Request.IsAjaxRequest())
            {
                var teacehrs = db.Teachers.OrderBy(t => t.LastName).Select(t => new { t.Id, t.LastName, t.FirstName }).ToList();
                return new JsonResult { Data = teacehrs, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            }
            return new JsonResult { Data = "error", JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        public JsonResult getGroups(string id)
        {
            int x = int.Parse(id);
            if (Request.IsAjaxRequest())
            {
                if (x == 0)
                {
                    var groups = db.Groups.OrderBy(t => t.Level).Select(t => new { t.Id, t.Name}).ToList();
                    return new JsonResult { Data = groups, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
                }
                else {
                    var groups = db.Groups.Where(g=> g.Teacher == x).OrderBy(t => t.Level).Select(t => new { t.Id, t.Name }).ToList();
                    return new JsonResult { Data = groups, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
                }
            }
            return new JsonResult { Data = "error", JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        public JsonResult getAttendances(string id)
        {
            int x = int.Parse(id);
            if (Request.IsAjaxRequest())
            {

                var attandances = db.Attendences.Where(a => a.Student == x).OrderBy(t => t.Id).Select(t => new { t.Id, t.Date, t.Present }).ToList();
                string date = string.Format("{0:dd.mm.yyyy}", attandances[1]);
                var count = attandances.Count();
                var array = new { attandances, count };
                return new JsonResult { Data = array, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            }
            return new JsonResult { Data = "error", JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        public JsonResult getGroupsByLevel(string id)
        {
            int x = int.Parse(id);
            if (Request.IsAjaxRequest())
            {
                if (x == 0)
                {
                    var groups = db.Groups.OrderBy(t => t.Level).Select(t => new { t.Id, t.Name }).ToList();
                    return new JsonResult { Data = groups, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
                }
                else
                {
                    var groups = db.Groups.Where(g => g.Level == x).OrderBy(t => t.Level).Select(t => new { t.Id, t.Name }).ToList();
                    return new JsonResult { Data = groups, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
                }
            }
            return new JsonResult { Data = "error", JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        public JsonResult getGroup(string id)
        {
            int x = int.Parse(id);
            if (Request.IsAjaxRequest())
            {
                
                    var group = db.Students.Where(g => g.Group == x).OrderBy(t => t.LastName).Select(t => new { t.Id, t.LastName, t.FirstName }).ToList();
                    return new JsonResult { Data = group, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
               
            }
            return new JsonResult { Data = "error", JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        public JsonResult getLevels()
        {            
            if (Request.IsAjaxRequest())
            {

                var group = db.Levels.OrderBy(t => t.Id).Select(t => new { t.Id, t.Name }).ToList();
                return new JsonResult { Data = group, JsonRequestBehavior = JsonRequestBehavior.AllowGet };

            }
            return new JsonResult { Data = "error", JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        [HttpPost]
        public JsonResult addStudent(string id, string LastName, string FirstName)
        {

            int x = int.Parse(id);
            using (FastAndEasyEntities db = new FastAndEasyEntities())
            {
                Student d = new Student();
                d.UserName = LastName+FirstName;
                d.Password = Crypto.HashPassword("1111");
                d.LastName = LastName;
                d.FirstName = FirstName;
                d.RoleId = 5;
                d.Group = x;
                db.Students.Add(d);
                db.SaveChanges();
                ModelState.Clear();
                return Json(d, JsonRequestBehavior.AllowGet);
            }
        }

        
        public JsonResult editStudent(string id, string LastName, string FirstName)
        {

            int x = int.Parse(id);
            using (FastAndEasyEntities db = new FastAndEasyEntities())
            {
                Student d = new Student();
                d = db.Students.Find(x);
                db.Students.Attach(d);
                d.LastName = LastName;
                d.FirstName = FirstName;
                db.Entry(d).Property(p => p.LastName).IsModified = true;
                db.Entry(d).Property(p => p.FirstName).IsModified = true;                
                db.SaveChanges();
                ModelState.Clear();
                return Json("", JsonRequestBehavior.AllowGet);

            }

        }

        public JsonResult deleteStudent(string id, string LastName, string FirstName)
        {

            int x = int.Parse(id);
            using (FastAndEasyEntities db = new FastAndEasyEntities())
            {
                Student d = new Student();
                d = db.Students.Find(x);
                db.Entry(d).State = EntityState.Deleted;
                db.SaveChanges();
                ModelState.Clear();
                return Json("", JsonRequestBehavior.AllowGet);

            }

        }

   
        public JsonResult studentPay(string sId, string groupId, string amount)
        {

            int sid = int.Parse(sId);
            int grpID = int.Parse(groupId);
            double sum = float.Parse(amount);
            using (FastAndEasyEntities db = new FastAndEasyEntities())
            {
                int? levelId = db.Groups.Where(g => g.Id == grpID).FirstOrDefault().Level;
                double? cost = db.Levels.Where(l => l.Id == levelId).FirstOrDefault().Price;
                var x = sum / cost;
                for(int i = 1; i <= x; i++)
                {
                    Attendence a = new Attendence();
                    a.Student = sid;
                    db.Attendences.Add(a);
                    db.SaveChanges();
                    ModelState.Clear();
                }




                return Json("success", JsonRequestBehavior.AllowGet);

            }

        }

        public JsonResult contentForNewGroup()
        {
            
            if (Request.IsAjaxRequest())
            {
                
                var teachers = db.Teachers.Select(t => new { t.Id, t.LastName, t.FirstName }).ToList();
                var levels = db.Levels.Select(t => new { t.Id, t.Name}).ToList();
                var content = new { teachers, levels };
                return new JsonResult { Data = content, JsonRequestBehavior = JsonRequestBehavior.AllowGet };

            }
            return new JsonResult { Data = "error", JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

    }
}