using FastAndEasy.Models;
using System;
using System.Linq;
using System.Web.Security;

namespace FastAndEasy.Providers
{
    public class CustomRoleProvider : RoleProvider
    {
        public override string[] GetRolesForUser(string email)
        {
            string[] role = new string[] { };
            using (FastAndEasyEntities _db = new FastAndEasyEntities())
            {
                try
                {
                    // Получаем пользователя
                    Admin admin = (from a in _db.Admins
                                 where a.Email == email || a.UserName == email
                                 select a).FirstOrDefault();
                    if (admin != null)
                    {
                        // получаем роль
                        Role adminRole = _db.Roles.Find(admin.RoleId);

                        if (adminRole != null)
                        {
                            role = new string[] { adminRole.Role1 };
                        }
                    }
                }
                catch
                {
                    role = new string[] { };
                }
            }

            using (FastAndEasyEntities _db = new FastAndEasyEntities())
            {
                try
                {
                    // Получаем пользователя
                    Manager director = (from d in _db.Managers
                                   where d.Email == email || d.UserName == email
                                   select d).FirstOrDefault();
                    if (director != null)
                    {
                        // получаем роль
                        Role directorRole = _db.Roles.Find(director.RoleId);

                        if (directorRole != null)
                        {
                            role = new string[] { directorRole.Role1 };
                        }
                    }
                }
                catch
                {
                    role = new string[] { };
                }
            }

            using (FastAndEasyEntities _db = new FastAndEasyEntities())
            {
                try
                {
                    // Получаем пользователя
                    Teacher teacher = (from t in _db.Teachers
                                   where t.Email == email || t.UserName == email
                                       select t).FirstOrDefault();
                    if (teacher != null)
                    {
                        // получаем роль
                        Role teacherRole = _db.Roles.Find(teacher.RoleId);

                        if (teacherRole != null)
                        {
                            role = new string[] { teacherRole.Role1 };
                        }
                    }
                }
                catch
                {
                    role = new string[] { };
                }
            }

            using (FastAndEasyEntities _db = new FastAndEasyEntities())
            {
                try
                {
                    // Получаем пользователя
                    Reception parent  = (from p in _db.Receptions
                                       where p.Email == email || p.UserName == email
                                       select p).FirstOrDefault();
                    if (parent != null)
                    {
                        // получаем роль
                        Role parentRole = _db.Roles.Find(parent.RoleId);

                        if (parentRole != null)
                        {
                            role = new string[] { parentRole.Role1 };
                        }
                    }
                }
                catch
                {
                    role = new string[] { };
                }
            }

            using (FastAndEasyEntities _db = new FastAndEasyEntities())
            {
                try
                {
                    // Получаем пользователя
                    Student pupil = (from p in _db.Students
                                       where p.UserName == email || p.Email == email
                                       select p).FirstOrDefault();
                    if (pupil != null)
                    {
                        // получаем роль
                        Role pupRole = _db.Roles.Find(pupil.RoleId);

                        if (pupRole != null)
                        {
                            role = new string[] { pupRole.Role1 };
                        }
                    }
                }
                catch
                {
                    role = new string[] { };
                }
            }



            return role;
        }
        public override void CreateRole(string roleName)
        {
            //Role newRole = new Role() { Name = roleName };
            //UserContext db = new UserContext();
            //db.Roles.Add(newRole);
            //db.SaveChanges();
        }
        public override bool IsUserInRole(string username, string roleName)
        {
            bool outputResult = false;
            //Находим пользователя
            using (FastAndEasyEntities _db = new FastAndEasyEntities())
            {
                try
                {
                    // Получаем пользователя
                    Admin admin = (from a in _db.Admins
                                 where a.Email == username || a.UserName == username
                                   select a).FirstOrDefault();
                    Manager director = (from d in _db.Managers
                                         where d.Email == username || d.UserName == username
                                         select d).FirstOrDefault();
                    Teacher teacher = (from t in _db.Teachers
                                   where t.Email == username || t.UserName == username
                                       select t).FirstOrDefault();
                    Reception parent = (from p in _db.Receptions
                                     where p.Email == username || p.UserName == username
                                     select p).FirstOrDefault();
                    Student pup = (from p in _db.Students
                                   where p.UserName == username || p.Email == username
                                 select p).FirstOrDefault();
                    if (admin != null)
                    {
                        // получаем роль
                        Role adminRole = _db.Roles.Find(admin.RoleId);

                        //сравниваем
                        if (adminRole != null && adminRole.Role1 == roleName)
                        {
                            outputResult = true;
                        }
                    }

                    if (director != null)
                    {
                        // получаем роль
                        Role directorRole = _db.Roles.Find(director.RoleId);

                        //сравниваем
                        if (directorRole != null && directorRole.Role1 == roleName)
                        {
                            outputResult = true;
                        }
                    }

                    if (teacher != null)
                    {
                        // получаем роль
                        Role teacherRole = _db.Roles.Find(teacher.RoleId);

                        //сравниваем
                        if (teacherRole != null && teacherRole.Role1 == roleName)
                        {
                            outputResult = true;
                        }
                    }

                    if (parent != null)
                    {
                        // получаем роль
                        Role parentRole = _db.Roles.Find(parent.RoleId);

                        //сравниваем
                        if (parentRole != null && parentRole.Role1 == roleName)
                        {
                            outputResult = true;
                        }
                    }

                    if (pup != null)
                    {
                        // получаем роль
                        Role pupRole = _db.Roles.Find(pup.RoleId);

                        //сравниваем
                        if (pupRole != null && pupRole.Role1 == roleName)
                        {
                            outputResult = true;
                        }
                    }

                }
                catch
                {
                    outputResult = false;
                }
            }
            return outputResult;
        }
        public override void AddUsersToRoles(string[] usernames, string[] roleNames)
        {
            throw new NotImplementedException();
        }

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

        public override bool DeleteRole(string roleName, bool throwOnPopulatedRole)
        {
            throw new NotImplementedException();
        }

        public override string[] FindUsersInRole(string roleName, string usernameToMatch)
        {
            throw new NotImplementedException();
        }

        public override string[] GetAllRoles()
        {
            throw new NotImplementedException();
        }

        public override string[] GetUsersInRole(string roleName)
        {
            throw new NotImplementedException();
        }

        public override void RemoveUsersFromRoles(string[] usernames, string[] roleNames)
        {
            throw new NotImplementedException();
        }

        public override bool RoleExists(string roleName)
        {
            throw new NotImplementedException();
        }
    }
}