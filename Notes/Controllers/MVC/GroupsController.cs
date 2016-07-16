using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Notes.Models;

namespace Notes.Controllers.MVC
{
    public class GroupsController : Controller
    {
        private NotesContext db = new NotesContext();


        //delete DelelteStudent:
        public ActionResult DeleteStudent(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var groupDetails = db.GroupDetails.Find(id);

            if (groupDetails == null)
            {
                return HttpNotFound();
            }

            db.GroupDetails.Remove(groupDetails);
            db.SaveChanges();


            return RedirectToAction($"Details/{groupDetails.GroupId}");
        }

        [HttpPost]
        public ActionResult AddStudent(GroupDetail groupDetail)
        {

            if (ModelState.IsValid)
            {

                //buscar si el estudiante ya existe en el grupo:
                var exist = db.GroupDetails.Where(gd => gd.GroupId == groupDetail.GroupId 
                            && gd.UserId == groupDetail.UserId).FirstOrDefault();

                if (exist == null)
                {
                    db.GroupDetails.Add(groupDetail);

                    db.SaveChanges();
                    return RedirectToAction($"Details/{groupDetail.GroupId}");
                }

                ModelState.AddModelError(string.Empty, "Estudiante ya matriculado en el Grupo.");
                                
               

               
            }

            ViewBag.UserId = new SelectList(db.Users.Where(u => u.IsStudent)
                          .OrderBy(u => u.FirstName)
                          .ThenBy(u => u.LastName), "UserId", "FullName", groupDetail.UserId);

            return View(groupDetail);
        }

        [HttpGet]
        public ActionResult AddStudent(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var group = db.Groups.Find(id);

            if (group == null)
            {
                return HttpNotFound();
            }

            var view = new GroupDetail
            {
                GroupId = id.Value,
            };

            ViewBag.UserId = new SelectList(db.Users.Where(u=>u.IsStudent)
                            .OrderBy(u=>u.FirstName)
                            .ThenBy(u=>u.LastName),"UserId","FullName");

            return View(view);
        }

        // GET: Groups
        public ActionResult Index()
        {
            var groups = db.Groups.Include(g => g.User);
            return View(groups.ToList());
        }

        // GET: Groups/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Group group = db.Groups.Find(id);
            if (group == null)
            {
                return HttpNotFound();
            }
            return View(group);
        }

        // GET: Groups/Create
        public ActionResult Create()
        {
            //filtro solo por profesores y nombre
            ViewBag.UserId = new SelectList(db.Users.Where(u=>u.IsTeacher).OrderBy(u=>u.LastName), "UserId", "FullName");
            return View();
        }

        // POST: Groups/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "GroupId,Description,UserId")] Group group)
        {
            if (ModelState.IsValid)
            {
                db.Groups.Add(group);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.UserId = new SelectList(db.Users.Where(u=>u.IsTeacher).OrderBy(u=>u.LastName), "UserId", "FullName", group.UserId);
            return View(group);
        }

        // GET: Groups/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Group group = db.Groups.Find(id);
            if (group == null)
            {
                return HttpNotFound();
            }
            ViewBag.UserId = new SelectList(db.Users.Where(u => u.IsTeacher).OrderBy(u => u.LastName), "UserId", "FullName", group.UserId);
            return View(group);
        }

        // POST: Groups/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "GroupId,Description,UserId")] Group group)
        {
            if (ModelState.IsValid)
            {
                db.Entry(group).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.UserId = new SelectList(db.Users, "UserId", "UserName", group.UserId);
            return View(group);
        }

        // GET: Groups/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Group group = db.Groups.Find(id);
            if (group == null)
            {
                return HttpNotFound();
            }
            return View(group);
        }

        // POST: Groups/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Group group = db.Groups.Find(id);
            db.Groups.Remove(group);
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
