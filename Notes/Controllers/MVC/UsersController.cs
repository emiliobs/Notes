using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Notes.Models;
using Notes.Classes;

namespace Notes.Controllers
{
    public class UsersController : Controller
    {
        private NotesContext db = new NotesContext();

        // GET: Users
        public ActionResult Index()
        {
            return View(db.Users.ToList());
        }

        // GET: Users/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = db.Users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        // GET: Users/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Users/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(UserView view)
        {
            if (ModelState.IsValid)
            {
                db.Users.Add(view.User);

                try
                {
                    db.SaveChanges();

                    //Pasos para subir la foto:                   

                    if (view.Photo != null)
                    {
                      var  pic = Utilities.UploadPhoto(view.Photo);

                        if (!string.IsNullOrEmpty(pic))
                        {
                            view.User.Photo = $"~/Content/Photos/{pic}";    
                        }

                    }

                    db.SaveChanges();

                    Utilities.CreateUserASP(view.User.UserName);
                    
                    //pregunto que roles tiene:

                    if (view.User.IsStudent)
                    {
                        Utilities.AddRoleToUser(view.User.UserName, "Student");
                    }

                    if (view.User.IsTeacher)
                    {
                        Utilities.AddRoleToUser(view.User.UserName,"Teacher");
                    }

                }
                catch (Exception ex)
                {

                    if (ex.InnerException != null &&  ex.InnerException.InnerException!= null
                        && ex.InnerException.InnerException.Message.Contains("index"))
                    {
                        ModelState.AddModelError(string.Empty,"There are record witn the same Name.");

                        return View(view);
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, ex.Message);

                        return View(view);
                    }
                }

                return RedirectToAction("Index");
            }

            return View(view);
        }

        // GET: Users/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = db.Users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }

            var view = new UserView
            {
                User = user,
            };

            return View(view);
        }

        // POST: Users/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(UserView view)
        {
            if (ModelState.IsValid)
            {
                db.Entry(view.User).State = EntityState.Modified;

                //Pasos para subir la foto:                   

                if (view.Photo != null)
                {
                    var pic = Utilities.UploadPhoto(view.Photo);

                    if (!string.IsNullOrEmpty(pic))
                    {
                        view.User.Photo = $"~/Content/Photos/{pic}";
                    }

                }

                try
                {
                    db.SaveChanges();

                   
                }
                catch (Exception ex)
                {

                    if (ex.InnerException != null && ex.InnerException.InnerException != null 
                        && ex.InnerException.Message.Contains("index"))
                    {
                        ModelState.AddModelError(string.Empty, "There are record witn the same records.");

                        return View(view);
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, ex.Message);

                        return View(view);
                    }
                }

                return RedirectToAction("Index");
            }
            return View(view);
        }

        // GET: Users/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = db.Users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        // POST: Users/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            User user = db.Users.Find(id);
            db.Users.Remove(user);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
