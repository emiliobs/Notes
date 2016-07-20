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
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;

namespace Notes.Controllers.API
{
    [RoutePrefix("API/Groups")]
    public class GroupsController : ApiController
    {
        private NotesContext db = new NotesContext();

        [HttpGet]
        [Route("GetNote/{groupId}/{userId}")]
        public IHttpActionResult GetNote(int groupId, int userId)
        {
            var noteDef = 0.0;
            var notes = db.GroupDetails.Where(gd => gd.GroupId == gd.GroupId && gd.UserId == userId).ToList();

            foreach (var  note in notes)
            {
                foreach (var note2 in note.Notes)
                {
                    noteDef += note2.Percentaje * note2.Qualification;
                }
            }

            return Ok<object>(new
            {
                Note = noteDef,
            });
        }

        [HttpPost]
        [Route("SaveNotes")]
        public IHttpActionResult SaveNotes(JObject form)
        {

            var myStudentsResponse = JsonConvert.DeserializeObject<MyStudentsResponse>(form.ToString());

            //transacciones en mvc:
            using (var transaction = db.Database.BeginTransaction())
            {
                try
                {
                    foreach (var student   in myStudentsResponse.Students)
                    {
                        var note = new Note
                        {
                                GroupDetailId = student.GroupDetailId,
                                Percentaje = (float)myStudentsResponse.Percentage,
                                Qualification = (float)student.Note,
                        };

                        db.Notes.Add(note);
                    }

                    db.SaveChanges();
                    transaction.Commit();

                    return Ok(true);
                }
                catch (Exception ex)
                {

                    transaction.Rollback();
                    return BadRequest(ex.Message);
                    
                }
            }

           
            //double percentage = 0;
            //var students = new List<MyStudent>();
            //dynamic jsonObject = form;

            //try
            //{
            //    percentage = jsonObject.Percentage.Value;
            //    students = jsonObject.Students.value;
            //}
            //catch 
            //{

            //    return this.BadRequest("Incorrect Call.");
            //}

        }


        [Route("GetStudents/{groupId}")]
        public IHttpActionResult GetStudents(int groupId)
        {

            var studnets = db.GroupDetails.Where(gd => gd.GroupId == groupId).ToList();

            var myStudents = new List<object>();
            foreach (var student in studnets)
            {
                //busco al studiante:
                var myStudent = db.Users.Find(student.UserId);
                myStudents.Add(new {

                    GroupDetailId = student.GroupDetailId,
                    GroupId = student.GroupId,
                    Student = myStudent,
                });
            }

            return Ok(myStudents);
        }

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

        [HttpPut]
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

        [HttpPost]
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

        [HttpDelete]
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