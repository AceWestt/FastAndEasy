
using FastAndEasy.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using System.Web.Security;

namespace FastAndEasy.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }
        [HttpPost]
        public ActionResult Index(LogOnModel model, string returnUrl, string password)
        {
            if (ModelState.IsValid)
            {

                // поиск пользователя в бд
                Admin admin = null;
                Manager director = null;
                Teacher teacher = null;
                Reception parent = null;
                Student pupil = null;
                using (FastAndEasyEntities db = new FastAndEasyEntities())
                {
                    admin = db.Admins.FirstOrDefault(a => a.Email == model.UserName || a.UserName == model.UserName);
                    director = db.Managers.FirstOrDefault(d => d.Email == model.UserName || d.UserName == model.UserName);
                    teacher = db.Teachers.FirstOrDefault(t => t.Email == model.UserName || t.UserName == model.UserName);
                    parent = db.Receptions.FirstOrDefault(p => p.Email == model.UserName || p.UserName == model.UserName);
                    pupil = db.Students.FirstOrDefault(p => p.UserName == model.UserName || p.Email == model.UserName);

                }
                if (admin != null && Crypto.VerifyHashedPassword(admin.Password, model.Password))
                {
                    FormsAuthentication.SetAuthCookie(model.UserName, true);
                    return RedirectToAction("Index", "Admin");
                }
                else
                {
                    ModelState.AddModelError("", "Пользователя с таким логином и паролем нет");
                }

                if (director != null && Crypto.VerifyHashedPassword(director.Password, model.Password))
                {
                    if (Crypto.VerifyHashedPassword(director.Password, "1111"))
                    {
                        FormsAuthentication.SetAuthCookie(model.UserName, true);
                        return RedirectToAction("EditFirstTime", "ForDirector", new { id = director.Id });
                    }
                    else
                    {
                        FormsAuthentication.SetAuthCookie(model.UserName, true);
                        return RedirectToAction("Index", "ForDirector", new { id = director.Id });
                    }
                }
                else
                {
                    ModelState.AddModelError("", "Пользователя с таким логином и паролем нет");
                }

                if (teacher != null && Crypto.VerifyHashedPassword(teacher.Password, model.Password))
                {
                    if (Crypto.VerifyHashedPassword(teacher.Password, "1111"))
                    {
                        FormsAuthentication.SetAuthCookie(model.UserName, true);
                        return RedirectToAction("EditFirstTime", "ForTeacher", new { id = teacher.Id });
                    }
                    else
                    {
                        FormsAuthentication.SetAuthCookie(model.UserName, true);
                        return RedirectToAction("Index", "ForTeacher", new { id = teacher.Id });
                    }

                }
                else
                {
                    ModelState.AddModelError("", "Пользователя с таким логином и паролем нет");
                }

                if (parent != null && Crypto.VerifyHashedPassword(parent.Password, model.Password))
                {
                    if (Crypto.VerifyHashedPassword(parent.Password, "1111"))
                    {
                        FormsAuthentication.SetAuthCookie(model.UserName, true);
                        return RedirectToAction("Edit", "Reception", new { id = parent.Id });
                    }
                    else
                    {
                        FormsAuthentication.SetAuthCookie(model.UserName, true);
                        return RedirectToAction("Index", "Reception", new { id = parent.Id });
                    }

                }
                else
                {
                    ModelState.AddModelError("", "Пользователя с таким логином и паролем нет");
                }

                if (pupil != null && Crypto.VerifyHashedPassword(pupil.Password, model.Password))
                {
                    if (Crypto.VerifyHashedPassword(pupil.Password, "1111"))
                    {
                        FormsAuthentication.SetAuthCookie(model.UserName, true);
                        return RedirectToAction("Edit", "Student", new { id = pupil.Id });
                    }
                    else
                    {
                        FormsAuthentication.SetAuthCookie(model.UserName, true);
                        return RedirectToAction("Index", "Student", new { id = pupil.Id });
                    }

                }
                else
                {
                    ModelState.AddModelError("", "Пользователя с таким логином и паролем нет");
                }

            }


            return View(model);
        }
        public ActionResult LogOff()
        {
            FormsAuthentication.SignOut();

            return RedirectToAction("Index", "Home");
        }


    }
}