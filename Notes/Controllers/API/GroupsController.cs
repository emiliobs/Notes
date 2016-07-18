using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using Notes.Models;

namespace Notes.Controllers.API
{
    [RoutePrefix("API/Groups")]
    public class GroupsController : ApiController
    {
        private NotesContext db = new NotesContext();

        [Route("GetGroups/{userId}")]
        public IHttpActionResult GetGroups(int userId)
        {
            //envio los grupos decada profesor:
            var groups = db.Groups.Where(g=>g.UserId == userId).ToList();

            var signatures = db.GroupDetails.Where(gd=>gd.UserId == userId).ToList();

            var matters = new List<object>();

            foreach (var signature in signatures)
            {
                //busco el profesor de la materia:
                var teacher = db.Users.Find(signature.Group.UserId);

                matters.Add
                    (new
                    {
                       GroupId = signature.GroupId,
                       Description = signature.Group.Description,
                       Teacher = teacher
                    }
                       );

                //matters.Add(db.Groups.Find(signature.GroupId));
            }
            //Encabezado para el JSon:
            var response = new
            {
                MyGroups = groups,
                MySignatures = matters,

            };

            return Ok(response);
        }

        // GET: api/Groups
        public IQueryable<Group> GetGroups()
        {
            return db.Groups;
        }

        // GET: api/Groups/5
        [ResponseType(typeof(Group))]
        public IHttpActionResult GetGroup(int id)
        {
            Group group = db.Groups.Find(id);
            if (group == null)
            {
                return NotFound();
            }

            return Ok(group);
        }

        // PUT: api/Groups/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutGroup(int id, Group group)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != group.GroupId)
            {
                return BadRequest();
            }

            db.Entry(group).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!GroupExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/Groups
        [ResponseType(typeof(Group))]
        public IHttpActionResult PostGroup(Group group)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Groups.Add(group);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = group.GroupId }, group);
        }

        // DELETE: api/Groups/5
        [ResponseType(typeof(Group))]
        public IHttpActionResult DeleteGroup(int id)
        {
            Group group = db.Groups.Find(id);
            if (group == null)
            {
                return NotFound();
            }

            db.Groups.Remove(group);
            db.SaveChanges();

            return Ok(group);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool GroupExists(int id)
        {
            return db.Groups.Count(e => e.GroupId == id) > 0;
        }
    }
}